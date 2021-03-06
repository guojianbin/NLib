﻿namespace NLib.ComponentModel.DataAnnotations.Tests
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Xunit;

    public class NotNotEqualsToAttributeTest
    {
        [Fact]
        public void PropertyAreEqualValue()
        {
            var model = new ModelProperty { P1 = "Foo", P2 = "Foo" };

            var vc1 = new ValidationContext(model, null, null);
            var vr = new List<ValidationResult>();
            var r = Validator.TryValidateObject(model, vc1, vr, true);
            Assert.False(r);
            Assert.Equal(1, vr.Count);
            Assert.Equal("'P2' and 'P1' must not match.", vr[0].ErrorMessage);
        }

        [Fact]
        public void PropertyAreNotEqualValue()
        {
            var model = new ModelProperty { P1 = "Foo", P2 = "Bar" };

            var vc1 = new ValidationContext(model, null, null);
            var vr = new List<ValidationResult>();
            var r = Validator.TryValidateObject(model, vc1, vr, true);
            Assert.True(r);
            Assert.Equal(0, vr.Count);
        }

        [Fact]
        public void PropertyDifferentTypeAreEqualValue()
        {
            var model = new ModelPropertyDifferentType { P1 = "2", P2 = 2 };

            var vc1 = new ValidationContext(model, null, null);
            var vr = new List<ValidationResult>();
            var r = Validator.TryValidateObject(model, vc1, vr, true);
            Assert.False(r);
            Assert.Equal(1, vr.Count);
            Assert.Equal("'P2' and 'P1' must not match.", vr[0].ErrorMessage);
        }

        [Fact]
        public void PropertyDifferentTypeAreNotEqualValue()
        {
            var model = new ModelPropertyDifferentType { P1 = "22", P2 = 2 };

            var vc1 = new ValidationContext(model, null, null);
            var vr = new List<ValidationResult>();
            var r = Validator.TryValidateObject(model, vc1, vr, true);
            Assert.True(r);
            Assert.Equal(0, vr.Count);
        }

        [Fact]
        public void PropertyCannotBeCompared()
        {
            var model = new ModelPropertyDifferentType { P1 = "Foo", P2 = 2 };

            var vc1 = new ValidationContext(model, null, null);
            var vr = new List<ValidationResult>();
            var ex = Assert.Throws<ValidationException>(() => Validator.TryValidateObject(model, vc1, vr, true));
            Assert.Equal("'P2' and 'P1' cannot be compared.", ex.Message);
        }

        [Fact]
        public void PropertyTypeMatch()
        {
            var model = new ModelPropertyTypeMatch { P1 = "Foo", P2 = "Foo" };

            var vc1 = new ValidationContext(model, null, null);
            var vr = new List<ValidationResult>();
            var r = Validator.TryValidateObject(model, vc1, vr, true);
            Assert.False(r);
            Assert.Equal(1, vr.Count);
            Assert.Equal("'P1' and 'P2' must not match.", vr[0].ErrorMessage);
        }

        [Fact]
        public void PropertyTypeMissMatch()
        {
            var model = new ModelPropertyTypeMissMatch { P1 = "Foo", P2 = 2 };

            var vc1 = new ValidationContext(model, null, null);
            var vr = new List<ValidationResult>();
            var ex = Assert.Throws<ValidationException>(() => Validator.TryValidateObject(model, vc1, vr, true));
            Assert.Equal("'P1' type (String) and 'P2' type (Int32) must be the same.", ex.Message);
        }

        [Fact]
        public void PropertyAreNull()
        {
            var model = new ModelProperty { P1 = null, P2 = null };

            var vc1 = new ValidationContext(model, null, null);
            var vr = new List<ValidationResult>();
            var r = Validator.TryValidateObject(model, vc1, vr, true);
            Assert.True(r);
            Assert.Equal(0, vr.Count);
        }

        [Fact]
        public void PropertyIsMissing()
        {
            var model = new ModelPropertyMissing { P1 = "Foo" };

            var vc1 = new ValidationContext(model, null, null);
            var vr = new List<ValidationResult>();
            var ex = Assert.Throws<ValidationException>(() => Validator.TryValidateObject(model, vc1, vr, true));
            Assert.Equal("Could not find a property named P2.", ex.Message);
        }

        public class ModelProperty
        {
            public string P1 { get; set; }
            [NotEqualsTo("P1")]
            public string P2 { get; set; }
        }

        public class ModelPropertyDifferentType
        {
            public string P1 { get; set; }
            [NotEqualsTo("P1")]
            public int P2 { get; set; }
        }

        public class ModelPropertyTypeMatch
        {
            [NotEqualsTo("P2", MustBeSameType = true)]
            public string P1 { get; set; }
            public string P2 { get; set; }
        }

        public class ModelPropertyTypeMissMatch
        {
            [NotEqualsTo("P2", MustBeSameType = true)]
            public string P1 { get; set; }
            public int P2 { get; set; }
        }

        public class ModelPropertyMissing
        {
            [EqualsTo("P2")]
            public string P1 { get; set; }
        }
    }
}
