using FileSystemSearchService.Core.Enums;
using FileSystemSearchService.Infrastructure.Services;
using Moq;
using System.IO.Abstractions;
using Xunit;

namespace FileSystemSearchService.Infrastructure.Tests.Services
{
    public class FilesAndFoldersServiceTests
    {
        FilesAndFoldersService _filesAndFolderService;

        Mock<IFileSystem> _mockIFileSystem;
        Mock<IFileInfo> _mockIFileInfo;

        public FilesAndFoldersServiceTests()
        {
            _mockIFileSystem = new Mock<IFileSystem>();
            _mockIFileInfo = new Mock<IFileInfo>();
        }

        [Fact]
        public void When_checking_if_IsFileOrFolder_and_Artifact_is_a_file()
        {
            //Arrange
            _mockIFileSystem.Setup(x => x.File.Exists(It.IsAny<string>())).Returns(true);

            _filesAndFolderService = new FilesAndFoldersService(_mockIFileSystem.Object);

            //Act
            var artifactTypeResult = _filesAndFolderService.IsFileOrFolder("Some string");

            //Assert
            Assert.Equal(ArtifactType.File, artifactTypeResult);
        }

        [Fact]
        public void When_checking_if_IsFileOrFolder_and_Artifact_is_a_folder()
        {
            //Arrange
            _mockIFileSystem.Setup(x => x.File.Exists(It.IsAny<string>())).Returns(false);

            _filesAndFolderService = new FilesAndFoldersService(_mockIFileSystem.Object);

            //Act
            var artifactTypeResult = _filesAndFolderService.IsFileOrFolder("Some string");

            //Assert
            Assert.Equal(ArtifactType.Folder, artifactTypeResult);
        }

        [Fact]
        public void When_requesting_to_GetFileInfo()
        {
            //Arrange
            var fullname = "TheFullName";
            var length = 123;
            var name = "TheName";

            _mockIFileInfo.Setup(x => x.FullName).Returns(fullname);
            _mockIFileInfo.Setup(x => x.Length).Returns(length);
            _mockIFileInfo.Setup(x => x.Name).Returns(name);

            _mockIFileSystem.Setup(x => x.FileInfo.FromFileName(It.IsAny<string>()))
                .Returns(_mockIFileInfo.Object);

            _filesAndFolderService = new FilesAndFoldersService(_mockIFileSystem.Object);

            //Act
            var ifileInfoResult = _filesAndFolderService.GetFileInfo("Some string");

            //Assert
            Assert.Equal(fullname, ifileInfoResult.FullName);
            Assert.Equal(length, ifileInfoResult.Length);
            Assert.Equal(name, ifileInfoResult.Name);
        }
    }
}
