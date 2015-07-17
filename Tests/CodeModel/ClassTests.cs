﻿using System.Linq;
using Should;
using Typewriter.CodeModel;
using Typewriter.CodeModel.CodeDom;
using Typewriter.CodeModel.Providers;
using Typewriter.Tests.TestInfrastructure;
using Xunit;

namespace Typewriter.Tests.CodeModel
{
    [Trait("Classes", "CodeDom")]
    public class CodeDomClassTests : ClassTests<CodeDomCodeModelProvider>
    {
    }

    [Trait("Classes", "Roslyn")]
    public class RoslynClassTests : ClassTests<RoslynProviderStub>
    {
    }

    public abstract class ClassTests<T> : TestBase<T> where T : ICodeModelProvider, new()
    {
        private readonly File fileInfo;

        protected ClassTests()
        {
            fileInfo = GetFile(@"Tests\CodeModel\Support\ClassInfo.cs");
        }

        [Fact]
        public void Expect_name_to_match_class_name()
        {
            var classInfo = fileInfo.Classes.First();

            classInfo.Name.ShouldEqual("ClassInfo");
            classInfo.FullName.ShouldEqual("Typewriter.Tests.CodeModel.Support.ClassInfo");
            classInfo.Namespace.ShouldEqual("Typewriter.Tests.CodeModel.Support");
            classInfo.Parent.ShouldEqual(fileInfo);
        }

        [Fact]
        public void Expect_to_find_attributes()
        {
            var classInfo = fileInfo.Classes.First();
            var attributeInfo = classInfo.Attributes.First();

            classInfo.Attributes.Count.ShouldEqual(1);
            attributeInfo.Name.ShouldEqual("AttributeInfo");
            attributeInfo.FullName.ShouldEqual("Typewriter.Tests.CodeModel.Support.AttributeInfoAttribute");
        }

        [Fact]
        public void Expect_to_find_base_class()
        {
            var classInfo = fileInfo.Classes.First();
            var baseClassInfo = classInfo.BaseClass;
            var propertyInfo = baseClassInfo.Properties.First();

            baseClassInfo.Name.ShouldEqual("BaseClassInfo");

            baseClassInfo.Properties.Count.ShouldEqual(1);
            propertyInfo.Name.ShouldEqual("PublicBaseProperty");
        }

        [Fact]
        public void Expect_not_to_find_object_base_class()
        {
            var classInfo = fileInfo.Classes.First(c => c.Name == "BaseClassInfo");

            classInfo.BaseClass.ShouldBeNull();
        }

        [Fact]
        public void Expect_to_find_interfaces()
        {
            var classInfo = fileInfo.Classes.First();
            var interfaceInfo = classInfo.Interfaces.First();
            var propertyInfo = interfaceInfo.Properties.First();

            classInfo.Interfaces.Count.ShouldEqual(1);
            interfaceInfo.Name.ShouldEqual("IInterfaceInfo");

            interfaceInfo.Properties.Count.ShouldEqual(1);
            propertyInfo.Name.ShouldEqual("PublicProperty");
        }

        [Fact]
        public void Expect_non_generic_class_not_to_be_generic()
        {
            var classInfo = fileInfo.Classes.First();

            classInfo.IsGeneric.ShouldBeFalse();
            classInfo.GenericTypeArguments.Count.ShouldEqual(0);
        }

        [Fact]
        public void Expect_generic_class_to_be_generic()
        {
            var classInfo = fileInfo.Classes.First(i => i.Name == "GenericClassInfo");
            var genericTypeArgument = classInfo.GenericTypeArguments.First();

            classInfo.IsGeneric.ShouldBeTrue();
            classInfo.GenericTypeArguments.Count.ShouldEqual(1);
            genericTypeArgument.Name.ShouldEqual("T");
        }

        [Fact]
        public void Expect_to_find_public_constants()
        {
            var classInfo = fileInfo.Classes.First();
            var constantInfo = classInfo.Constants.First();

            classInfo.Constants.Count.ShouldEqual(1);
            constantInfo.Name.ShouldEqual("PublicConstant");
        }

        [Fact]
        public void Expect_to_find_public_fields()
        {
            var classInfo = fileInfo.Classes.First();
            var fieldInfo = classInfo.Fields.First();

            classInfo.Fields.Count.ShouldEqual(1);
            fieldInfo.Name.ShouldEqual("PublicField");
        }

        [Fact]
        public void Expect_to_find_public_methods()
        {
            var classInfo = fileInfo.Classes.First();
            var methodInfo = classInfo.Methods.First();

            classInfo.Methods.Count.ShouldEqual(1);
            methodInfo.Name.ShouldEqual("PublicMethod");
        }

        [Fact]
        public void Expect_to_find_public_properties()
        {
            var classInfo = fileInfo.Classes.First();
            var propertyInfo = classInfo.Properties.First();

            classInfo.Properties.Count.ShouldEqual(1);
            propertyInfo.Name.ShouldEqual("PublicProperty");
        }

        [Fact]
        public void Expect_to_find_nested_public_classes()
        {
            var classInfo = fileInfo.Classes.First();
            var nestedClassInfo = classInfo.NestedClasses.First();
            var propertyInfo = nestedClassInfo.Properties.First();

            classInfo.NestedClasses.Count.ShouldEqual(1);
            nestedClassInfo.Name.ShouldEqual("NestedClassInfo");

            nestedClassInfo.Properties.Count.ShouldEqual(1);
            propertyInfo.Name.ShouldEqual("PublicNestedProperty");
        }

        [Fact]
        public void Expect_to_find_nested_public_enums()
        {
            var classInfo = fileInfo.Classes.First();
            var nestedEnumInfo = classInfo.NestedEnums.First();
            var valueInfo = nestedEnumInfo.Values.First();

            classInfo.NestedEnums.Count.ShouldEqual(1);
            nestedEnumInfo.Name.ShouldEqual("NestedEnumInfo");

            nestedEnumInfo.Values.Count.ShouldEqual(1);
            valueInfo.Name.ShouldEqual("NestedValue");
        }

        [Fact]
        public void Expect_to_find_nested_public_interfaces()
        {
            var classInfo = fileInfo.Classes.First();
            var nestedInterfaceInfo = classInfo.NestedInterfaces.First();
            var propertyInfo = nestedInterfaceInfo.Properties.First();

            classInfo.NestedInterfaces.Count.ShouldEqual(1);
            nestedInterfaceInfo.Name.ShouldEqual("INestedInterfaceInfo");

            nestedInterfaceInfo.Properties.Count.ShouldEqual(1);
            propertyInfo.Name.ShouldEqual("PublicNestedProperty");
        }

        [Fact]
        public void Expect_to_find_containing_class_on_nested_class()
        {
            var classInfo = fileInfo.Classes.First();
            var nestedClassInfo = classInfo.NestedClasses.First();
            var containingClassInfo = nestedClassInfo.ContainingClass;

            containingClassInfo.Name.ShouldEqual("ClassInfo");
        }

        [Fact]
        public void Expect_not_to_find_containing_class_on_top_level_class()
        {
            var classInfo = fileInfo.Classes.First();
            var containingClassInfo = classInfo.ContainingClass;

            containingClassInfo.ShouldBeNull();
        }
    }
}