﻿namespace NLib.Tests.Patterns
{
    using NLib.Patterns;

    using NUnit.Framework;

    [TestFixture]
    public class SimpleCommandTest
    {
        [Test]
        public void Test1()
        {
            var i = 1;
            var sc = new SimpleCommand(() => i++, () => i--);

            sc.Execute();
            Assert.AreEqual(2, i);
            Assert.True(sc.CanUndo);
            Assert.False(sc.CanRedo);

            sc.Execute();
            Assert.AreEqual(3, i);
            Assert.True(sc.CanUndo);
            Assert.False(sc.CanRedo);

            sc.Undo();
            Assert.AreEqual(2, i);
            Assert.True(sc.CanUndo);
            Assert.True(sc.CanRedo);

            sc.Undo();
            Assert.AreEqual(1, i);
            Assert.False(sc.CanUndo);
            Assert.True(sc.CanRedo);

            sc.Redo();
            Assert.AreEqual(2, i);
            Assert.True(sc.CanUndo);
            Assert.True(sc.CanRedo);

            sc.Redo();
            Assert.AreEqual(3, i);
            Assert.True(sc.CanUndo);
            Assert.False(sc.CanRedo);
        }

        [Test]
        public void Test2()
        {
            var i = 1;
            var sc = new SimpleCommand(() => i++, () => i--);

            sc.Execute();
            sc.Execute();

            sc.Clear();

            Assert.False(sc.CanUndo);
            Assert.False(sc.CanRedo);
        }
    }
}
