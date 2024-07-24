using Metalama.Framework.Advising;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code;

namespace Memento.Step2;

public sealed class MementoAttribute : TypeAspect
{
    [CompileTime]
    private record BuildAspectInfo( INamedType SnapshotType, Dictionary<IFieldOrProperty, IProperty> PropertyMap, IProperty OriginatorProperty );

    public override void BuildAspect( IAspectBuilder<INamedType> builder )
    {
        // Introduce a new private nested class called Memento.
        var mementoType =
            builder.IntroduceClass(
                "Memento",
                buildType: b => b.Accessibility = Metalama.Framework.Code.Accessibility.Private );

        // Introduce originator property that will hold a reference to the instance that created the Memento.
        var originatorProperty =
            mementoType.IntroduceProperty( nameof(Originator) );

        // Dictionary that maps fields of the target class to memento properties.
        var propertyMap = new Dictionary<IFieldOrProperty, IProperty>();

        // Introduce data properties to the Memento class for each field of the target class.
        foreach (var fieldOrProperty in builder.Target.FieldsAndProperties)
        {
            if (fieldOrProperty is not { IsAutoPropertyOrField: true, IsImplicitlyDeclared: false })
            {
                // Ignore properties that are not auto-properties and fields that are not explicitly declared.
                continue;
            }

            if (fieldOrProperty.Writeability is not Writeability.All ||
                fieldOrProperty.Attributes.OfAttributeType( typeof(MementoIgnoreAttribute) ).Any())
            {
                // Ignore read-only declarations and those marked with the MementoIgnore attribute.
                continue;
            }

            var introducedField = mementoType.IntroduceProperty(
                nameof(MementoProperty),
                buildProperty: b =>
                {
                    var trimmedName = fieldOrProperty.Name.TrimStart( '_' );

                    b.Name = trimmedName.Substring( 0, 1 ).ToUpperInvariant() + trimmedName.Substring( 1 );
                    b.Type = fieldOrProperty.Type;
                } );

            propertyMap.Add( fieldOrProperty, introducedField.Declaration );
        }

        // Implement the ISnapshot interface on the Snapshot class.
        mementoType.ImplementInterface( typeof(IMemento), whenExists: OverrideStrategy.Ignore );

        // Add a constructor to the Memento class that records the state of the .
        mementoType.IntroduceConstructor(
            nameof(MementoConstructorTemplate),
            buildConstructor: b => { b.AddParameter( "originator", builder.Target ); } );

        // Introduce a Restore method to the target class, that loads the state of the object from a Memento.
        builder.IntroduceMethod(
            nameof(SaveToMemento),
            whenExists: OverrideStrategy.Override,
            args: new { mementoType = mementoType.Declaration } );

        // Introduce a Restore method to the target class, that loads the state of the object from a Memento.
        builder.IntroduceMethod(
            nameof(RestoreMemento),
            whenExists: OverrideStrategy.Override );

        // Implement the rest of the IOriginator interface.
        builder.ImplementInterface( typeof(IMementoable) );

        builder.Tags = new BuildAspectInfo( mementoType.Declaration, propertyMap, originatorProperty.Declaration );
    }

    [Template]
    public object? MementoProperty { get; }

    [Template]
    public IMementoable? Originator { get; }

    [Template]
    public IMemento SaveToMemento()
    {
        var buildAspectInfo = (BuildAspectInfo)meta.Tags.Source!;

        // Invoke the constructor of the Memento class and pass this object as the originator.
        return buildAspectInfo.SnapshotType.Constructors.Single().Invoke( (IExpression)meta.This )!;
    }

    [Template]
    public void RestoreMemento( IMemento memento )
    {
        var buildAspectInfo = (BuildAspectInfo)meta.Tags.Source!;

        var typedSnapshot = meta.Cast( buildAspectInfo.SnapshotType, memento );

        // Set fields of this instance to the values stored in the Snapshot.
        foreach (var pair in buildAspectInfo.PropertyMap)
        {
            pair.Key.Value = pair.Value.With( (IExpression) typedSnapshot ).Value;
        }
    }

    [Template]
    public void MementoConstructorTemplate()
    {
        var buildAspectInfo = (BuildAspectInfo)meta.Tags.Source!;

        // Set the originator property and the data properties of the Snapshot.
        buildAspectInfo.OriginatorProperty.Value = meta.Target.Parameters[0];

        foreach (var pair in buildAspectInfo.PropertyMap)
        {
            pair.Value.Value = pair.Key.With( meta.Target.Parameters[0] ).Value;
        }
    }
}