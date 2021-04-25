using FileSystemSearchService.Core.DTO.FileSystem;
using FileSystemSearchService.Core.Entities;
using FileSystemSearchService.Core.Enums;
using FileSystemSearchService.Infrastructure.Repositories;
using FileSystemSearchService.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Nest;
using System;
using System.IO.Abstractions;
using Xunit;

namespace FileSystemSearchService.Infrastructure.Tests.Services
{
    public class ArtifactIndexingSearchServiceTests
    {
        Mock<ILogger<ArtifactIndexingService>> _mockILogger;
        Mock<ArtifactRepository> _mockArtifactRepository;
        Mock<FilesAndFoldersService> _mockFilesAndFoldersService;
        Mock<IFileInfo> _mockIFileInfo;
        Mock<IFileSystem> _mockIFileSystem;

        public ArtifactIndexingSearchServiceTests()
        {
            _mockIFileInfo = new Mock<IFileInfo>();
            _mockIFileSystem = new Mock<IFileSystem>();

            _mockILogger = new Mock<ILogger<ArtifactIndexingService>>();
            _mockArtifactRepository = new Mock<ArtifactRepository>(It.IsAny<IElasticClient>());
            _mockFilesAndFoldersService = new Mock<FilesAndFoldersService>(_mockIFileSystem.Object);
        }

        [Fact]
        public void When_a_delta_update_is_requested_on_a_created_event_and_everything_is_fine()
        {
            //Arrange
            var createdEventDTO = new CreatedEventDTO
            {
                ArtifactType = ArtifactType.File,
                FullPath = "C:\\TheFullPath\\TheFileName.txt",
                Name = "TheFileName.txt"
            };

            _mockFilesAndFoldersService.Setup(x => x.GetFileInfo(It.IsAny<string>()))
                .Returns(_mockIFileInfo.Object);

            var artifactIndexingService = new ArtifactIndexingService(_mockILogger.Object,
                _mockArtifactRepository.Object,
                _mockFilesAndFoldersService.Object);

            //Act
            artifactIndexingService.DeltaUpdate(createdEventDTO);

            //Assert
            _mockFilesAndFoldersService.Verify(x => x.GetFileInfo(It.IsAny<string>()), Times.Once);
            _mockArtifactRepository.Verify(x => x.Add(It.IsAny<Artifact>()), Times.Once);
        }

        [Fact]
        public void When_a_delta_update_is_requested_on_a_change_event_and_everything_is_fine()
        {
            //Arrange
            var changedEventDTO = new ChangedEventDTO
            {
                ArtifactType = ArtifactType.File,
                FullPath = "C:\\TheFullPath\\TheFileName.txt",
                Name = "TheFileName.txt"
            };

            _mockFilesAndFoldersService.Setup(x => x.GetFileInfo(It.IsAny<string>()))
                .Returns(_mockIFileInfo.Object);

            var artifactIndexingService = new ArtifactIndexingService(_mockILogger.Object,
                _mockArtifactRepository.Object,
                _mockFilesAndFoldersService.Object);

            //Act
            artifactIndexingService.DeltaUpdate(changedEventDTO);

            //Assert
            _mockFilesAndFoldersService.Verify(x => x.GetFileInfo(It.IsAny<string>()), Times.Once);

            _mockFilesAndFoldersService.VerifyNoOtherCalls();
            _mockArtifactRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public void When_a_delta_update_is_requested_on_a_renamed_event_and_everything_is_fine()
        {
            //Arrange
            var renamedEventDTO = new RenamedEventDTO
            {
                ArtifactType = ArtifactType.File,
                FullPath = "C:\\TheFullPath\\TheFileName.txt",
                Name = "TheFileName.txt",
                OldFullPath = "C:\\TheOldFullPath\\TheOldFullName.txt",
                OldName = "TheOldFullName.txt"
            };

            _mockFilesAndFoldersService.Setup(x => x.GetFileInfo(It.IsAny<string>()))
                .Returns(_mockIFileInfo.Object);

            var artifactIndexingService = new ArtifactIndexingService(_mockILogger.Object,
                _mockArtifactRepository.Object,
                _mockFilesAndFoldersService.Object);

            //Act
            artifactIndexingService.DeltaUpdate(renamedEventDTO);

            //Assert
            _mockFilesAndFoldersService.Verify(x => x.GetFileInfo(It.IsAny<string>()), Times.Once);
            _mockArtifactRepository.Verify(x => x.Delete(It.IsAny<string>()), Times.Once);
            _mockArtifactRepository.Verify(x => x.Add(It.IsAny<Artifact>()), Times.Once);
        }

        [Fact]
        public void When_a_delta_update_is_requested_on_a_delete_event_and_everything_is_fine()
        {
            //Arrange
            var renamedEventDTO = new RenamedEventDTO
            {
                ArtifactType = ArtifactType.File,
                FullPath = "C:\\TheFullPath\\TheFileName.txt",
                Name = "TheFileName.txt",
                OldFullPath = "C:\\TheOldFullPath\\TheOldFullName.txt",
                OldName = "TheOldFullName.txt"
            };

            _mockFilesAndFoldersService.Setup(x => x.GetFileInfo(It.IsAny<string>()))
                .Returns(_mockIFileInfo.Object);

            var artifactIndexingService = new ArtifactIndexingService(_mockILogger.Object,
                _mockArtifactRepository.Object,
                _mockFilesAndFoldersService.Object);

            //Act
            artifactIndexingService.DeltaUpdate(renamedEventDTO);

            //Assert
            _mockFilesAndFoldersService.Verify(x => x.GetFileInfo(It.IsAny<string>()), Times.Once);
            _mockArtifactRepository.Verify(x => x.Delete(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void When_a_full_rebuild_is_requested()
        {
            //Arrange
            var artifactIndexingService = new ArtifactIndexingService(_mockILogger.Object,
                _mockArtifactRepository.Object,
                _mockFilesAndFoldersService.Object);

            //Act

            //Assert
            Assert.Throws<NotImplementedException>(() =>
                artifactIndexingService.Rebuild());
        }
    }
}
