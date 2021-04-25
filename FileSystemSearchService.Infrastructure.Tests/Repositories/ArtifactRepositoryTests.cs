using FileSystemSearchService.Core.Entities;
using FileSystemSearchService.Infrastructure.Repositories;
using Moq;
using Nest;
using System;
using System.Collections.Generic;
using Xunit;

namespace FileSystemSearchService.Infrastructure.Tests.Repositories
{
    public class ArtifactRepositoryTests
    {
        Mock<IElasticClient> _mockIElasticClient;
        ArtifactRepository _artifactRepository;

        public ArtifactRepositoryTests()
        {
            _mockIElasticClient = new Mock<IElasticClient>();
            _artifactRepository = new ArtifactRepository(_mockIElasticClient.Object);
        }

        [Fact]
        public void When_attempting_to_add_a_single_Artifact_and_it_is_successful()
        {
            //Arrange
            var artifactToAdd = new Artifact
            {
                Bytes = 100,
                Created = DateTime.Now,
                FileType = ".txt",
                FullPath = "C:\\MyFolder\\MyFile.txt",
                LastAccessed = DateTime.Now,
                Modified = DateTime.Now,
                Name = "MyFile.txt",
                Path = "C:\\MyFolder"
            };

            var reflectedIndexResponse = new IndexResponse();
            var reflectedId = reflectedIndexResponse.GetType().GetProperty(nameof(IndexResponse.Id));
            reflectedId.SetValue(reflectedIndexResponse, artifactToAdd.FullPath);

            _mockIElasticClient.Setup(x => x.IndexDocument(artifactToAdd))
                .Returns(reflectedIndexResponse);

            //Act
            var result = _artifactRepository.Add(artifactToAdd);

            //Assert
            Assert.IsType<string>(result);
            Assert.True(String.Equals(artifactToAdd.FullPath, result, StringComparison.Ordinal));
        }

        [Fact]
        public void When_attempting_to_add_a_single_Artifact_and_it_is_unsuccessful()
        {
            //todo: Implement this test.
        }

        [Fact]
        public void When_attempting_to_bulk_add_a_collection_of_Artifacts_and_it_is_successful()
        {
            Assert.Throws<NotImplementedException>(() =>
                _artifactRepository.BulkAdd(It.IsAny<IEnumerable<Artifact>>()));
        }

        [Fact]
        public void When_attempting_to_bulk_add_a_collection_of_Artifacts_and_it_is_unsuccessful()
        {
            Assert.Throws<NotImplementedException>(() =>
                _artifactRepository.BulkAdd(It.IsAny<IEnumerable<Artifact>>()));
        }

        [Fact]
        public void When_attempting_to_delete_a_single_Artifact_given_its_id_and_it_is_successful()
        {
            //Arrange
            var reflectedDeleteResponse = new DeleteResponse();
            var reflectedResult = reflectedDeleteResponse.GetType().GetProperty(nameof(DeleteResponse.Result), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            reflectedResult.SetValue(reflectedDeleteResponse, Result.Deleted);

            _mockIElasticClient.Setup(x => x.Delete<Artifact>(It.IsAny<DocumentPath<Artifact>>(), null))
                .Returns(reflectedDeleteResponse);

            //Act
            var result = _artifactRepository.Delete(It.IsAny<string>());

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void When_attempting_to_delete_a_single_Artifact_given_its_id_and_it_is_unsuccessful()
        {
            //Arrange
            var reflectedDeleteResponse = new DeleteResponse();
            var reflectedResult = reflectedDeleteResponse.GetType().GetProperty(nameof(DeleteResponse.Result));
            reflectedResult.SetValue(reflectedDeleteResponse, Result.Error);

            _mockIElasticClient.Setup(x => x.Delete<Artifact>(It.IsAny<DocumentPath<Artifact>>(), null))
                .Returns(reflectedDeleteResponse);

            //Act
            var result = _artifactRepository.Delete(It.IsAny<string>());

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void When_attempting_to_update_a_single_Artifact_given_its_id_and_it_is_successful()
        {
            Assert.Throws<NotImplementedException>(() =>
                _artifactRepository.Update(It.IsAny<string>(), It.IsAny<Artifact>()));
        }

        [Fact]
        public void When_attempting_to_update_a_single_Artifact_given_its_id_and_it_is_unsuccessful()
        {
            Assert.Throws<NotImplementedException>(() =>
                _artifactRepository.Update(It.IsAny<string>(), It.IsAny<Artifact>()));
        }
    }
}
