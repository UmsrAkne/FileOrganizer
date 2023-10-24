using System.Collections.Generic;
using FileOrganizer.Models;
using NUnit.Framework;

namespace FileOrganizerTest.Models
{
    [TestFixture]
    public class FileContainerTest
    {
        [Test]
        public void MoveCursorCommandTest_要素なし()
        {
            var fileContainer = new FileContainer(new List<ExtendFileInfo>());
            Assert.That(fileContainer.CursorIndex, Is.EqualTo(-1), "デフォルトのインデックスは -1");

            // 以下ではコマンドを実行しても、リストが空であるためインデックスは変化しないはず。
            fileContainer.MoveCursorCommand.Execute(1);
            Assert.That(fileContainer.CursorIndex, Is.EqualTo(-1));

            fileContainer.MoveCursorCommand.Execute(0);
            Assert.That(fileContainer.CursorIndex, Is.EqualTo(-1));

            fileContainer.MoveCursorCommand.Execute(-1);
            Assert.That(fileContainer.CursorIndex, Is.EqualTo(-1));
        }

        [Test]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-3)]
        public void MoveCursorCommandTest_要素１(int moveCount)
        {
            var fileContainer = new FileContainer(new List<ExtendFileInfo>() { new ExtendFileInfo("c:\\"), });
            Assert.That(fileContainer.CursorIndex, Is.EqualTo(0), "要素が１以上の場合はインデックスが 0 にセットされる");

            fileContainer.MoveCursorCommand.Execute(moveCount);
            Assert.That(fileContainer.CursorIndex, Is.EqualTo(0), "どのような値が来ても、カーソルが動く余地はないので、値は変化しない");
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(3, 1)]
        [TestCase(0, 0)]
        [TestCase(-1, 0)]
        [TestCase(-3, 0)]
        public void MoveCursorCommandTest_要素２(int moveCount, int afterIndex)
        {
            var fileContainer = new FileContainer(new List<ExtendFileInfo>()
            {
                new ExtendFileInfo("c:\\"),
                new ExtendFileInfo("c:\\"),
            });

            Assert.That(fileContainer.CursorIndex, Is.EqualTo(0), "要素が１以上の場合はインデックスが 0 にセットされる");

            fileContainer.MoveCursorCommand.Execute(moveCount);
            Assert.That(fileContainer.CursorIndex, Is.EqualTo(afterIndex));
        }

        [Test]
        public void ReloadCommandTest_StartIndex0()
        {
            var files = new List<ExtendFileInfo>()
            {
                new ExtendFileInfo("c:\\a"),
                new ExtendFileInfo("c:\\b"),
                new ExtendFileInfo("c:\\c"),
            };

            var fileContainer = new FileContainer(files) { StartIndex = 0, };
            fileContainer.ReloadCommand.Execute();

            Assert.That(files[0].Index, Is.EqualTo(0));
            Assert.That(files[1].Index, Is.EqualTo(1));
            Assert.That(files[2].Index, Is.EqualTo(2));
        }

        [Test]
        public void ReloadCommandTest_StartIndex1()
        {
            var files = new List<ExtendFileInfo>()
            {
                new ExtendFileInfo("c:\\a"),
                new ExtendFileInfo("c:\\b"),
                new ExtendFileInfo("c:\\c"),
            };

            var fileContainer = new FileContainer(files) { StartIndex = 1, };

            fileContainer.ReloadCommand.Execute();

            Assert.That(files[0].Index, Is.EqualTo(1));
            Assert.That(files[1].Index, Is.EqualTo(2));
            Assert.That(files[2].Index, Is.EqualTo(3));
        }

        [Test]
        public void ReloadCommandTest_無視ファイルを含む()
        {
            var files = new List<ExtendFileInfo>()
            {
                new ExtendFileInfo("c:\\a") { Index = 0, },
                new ExtendFileInfo("c:\\b") { Index = 0, Ignore = true, },
                new ExtendFileInfo("c:\\c") { Index = 0, },
            };

            var fileContainer = new FileContainer(files) { StartIndex = 1, };

            fileContainer.ReloadCommand.Execute();

            Assert.That(files[0].Index, Is.EqualTo(1));
            Assert.That(files[1].Index, Is.EqualTo(0), "このオブジェクトはカウントに含まれないので 0 のまま");
            Assert.That(files[2].Index, Is.EqualTo(2));
        }
    }
}