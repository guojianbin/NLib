﻿namespace NLib.ComponentModel.DataAnnotations.Tests
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using NUnit.Framework;

    [TestFixture]
    public class GreaterThanAttributeTest
    {
        [Test]
        public void PropertyAreEqualValue()
        {
            var model = new ModelProperty { P1 = "10", P2 = "3" };

            var vc1 = new ValidationContext(model, null, null);
            var vr = new List<ValidationResult>();
            var r = Validator.TryValidateObject(model, vc1, vr, true);
            Assert.True(r);
            Assert.AreEqual(0, vr.Count);
        }

        [Test]
        public void PropertyAreNotEqualValue()
        {
            var model = new ModelProperty { P1 = "9", P2 = "6" };

            var vc1 = new ValidationContext(model, null, null);
            var vr = new List<ValidationResult>();
            var r = Validator.TryValidateObject(model, vc1, vr, true);
            Assert.False(r);
            Assert.AreEqual(1, vr.Count);
            Assert.AreEqual("'P2' must be greater than 'P1'.", vr[0].ErrorMessage);
        }

        [Test]
        public void PropertyDifferentTypeAreEqualValue()
        {
            var model = new ModelPropertyDifferentType { P1 = "3", P2 = 12 };

            var vc1 = new ValidationContext(model, null, null);
            var vr = new List<ValidationResult>();
            var r = Validator.TryValidateObject(model, vc1, vr, true);
            Assert.True(r);
            Assert.AreEqual(0, vr.Count);
        }

        [Test]
        public void PropertyDifferentTypeAreNotEqualValue()
        {
            var model = new ModelPropertyDifferentType { P1 = "22", P2 = 2 };

            var vc1 = new ValidationContext(model, null, null);
            var vr = new List<ValidationResult>();
            var r = Validator.TryValidateObject(model, vc1, vr, true);
            Assert.False(r);
            Assert.AreEqual(1, vr.Count);
            Assert.AreEqual("'P2' must be greater than 'P1'.", vr[0].ErrorMessage);
        }

        [Test]
        [ExpectedException(typeof(ValidationException), ExpectedMessage = "'P2' and 'P1' cannot be compared.")]
        public void PropertyCannotBeCompared()
        {
            var model = new ModelPropertyDifferentType { P1 = "Foo", P2 = 2 };

            var vc1 = new ValidationContext(model, null, null);
            var vr = new List<ValidationResult>();
            Validator.TryValidateObject(model, vc1, vr, true);
            Assert.Fail();
        }

        [Test]
        public void PropertyTypeMatch()
        {
            var model = new ModelPropertyTypeMatch { P1 = "8", P2 = "5" };

            var vc1 = new ValidationContext(model, null, null);
            var vr = new List<ValidationResult>();
            var r = Validator.TryValidateObject(model, vc1, vr, true);
            Assert.True(r);
            Assert.AreEqual(0, vr.Count);
        }

        [Test]
        [ExpectedException(typeof(ValidationException), ExpectedMessage = "'P1' type (String) and 'P2' type (Int32) must be the same.")]
        public void PropertyTypeMissMatch()
        {
            var model = new ModelPropertyTypeMissMatch { P1 = "7", P2 = 2 };

            var vc1 = new ValidationContext(model, null, null);
            var vr = new List<ValidationResult>();
            Validator.TryValidateObject(model, vc1, vr, true);
            Assert.Fail();
        }

        [Test]
        public void PropertyAreNull()
        {
            var model = new ModelProperty { P1 = null, P2 = null };

            var vc1 = new ValidationContext(model, null, null);
            var vr = new List<ValidationResult>();
            var r = Validator.TryValidateObject(model, vc1, vr, true);
            Assert.True(r);
            Assert.AreEqual(0, vr.Count);
        }

        [Test]
        [ExpectedException(typeof(ValidationException), ExpectedMessage = "Could not find a property named P2.")]
        public void PropertyIsMissing()
        {
            var model = new ModelPropertyMissing { P1 = "Foo" };

            var vc1 = new ValidationContext(model, null, null);
            var vr = new List<ValidationResult>();
            Validator.TryValidateObject(model, vc1, vr, true);
            Assert.Fail();
        }

        public class ModelProperty
        {
            public string P1 { get; set; }
            [GreaterThan("P1")]
            public string P2 { get; set; }
        }

        public class ModelPropertyDifferentType
        {
            public string P1 { get; set; }
            [GreaterThan("P1")]
            public int P2 { get; set; }
        }

        public class ModelPropertyTypeMatch
        {
            [GreaterThan("P2", MustBeSameType = true)]
            public string P1 { get; set; }
            public string P2 { get; set; }
        }

        public class ModelPropertyTypeMissMatch
        {
            [GreaterThan("P2", MustBeSameType = true)]
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