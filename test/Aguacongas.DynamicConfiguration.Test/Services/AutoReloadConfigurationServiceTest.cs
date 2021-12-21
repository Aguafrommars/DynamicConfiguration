using Aguacongas.DynamicConfiguration.Options;
using Aguacongas.DynamicConfiguration.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using Xunit;

namespace Aguacongas.DynamicConfiguration.Test.Services
{
    public class AutoReloadConfigurationServiceTest
    {
        [Fact]
        public void Construtor_should_verify_arguments()
        {
            Assert.Throws<ArgumentNullException>(() => new AutoReloadConfigurationService(null, null));
            Assert.Throws<ArgumentNullException>(() => new AutoReloadConfigurationService(new Mock<IConfigurationRoot>().Object, null));
        }

        [Fact]
        public void SubscribeToChanges_should_get_provider_reload_token_and_reload_root_configuration()
        {
            var configurationRootMock = new Mock<IConfigurationRoot>();
            configurationRootMock.Setup(m => m.Reload()).Verifiable();

            Action<object>? callback = null;
            var disposableMock = new Mock<IDisposable>();
            disposableMock.Setup(m => m.Dispose()).Verifiable();

            var changeTokenMock = new Mock<IChangeToken>();
            changeTokenMock.Setup(m => m.RegisterChangeCallback(It.IsAny<Action<object>>(), It.IsAny<object>()))
                .Callback<Action<object>, object>((action, state) => callback = action)
                .Returns(disposableMock.Object);

            var providerMock = new Mock<IConfigurationProvider>();
            providerMock.Setup(m => m.GetReloadToken()).Returns(changeTokenMock.Object);

            using var sut = new AutoReloadConfigurationService(configurationRootMock.Object, Microsoft.Extensions.Options.Options.Create(new DynamicConfigurationOptions
            {
                Provider = providerMock.Object
            }));

            sut.SubscribeToChanges();

            Assert.NotNull(callback);

            callback?.Invoke(new object());

            configurationRootMock.Verify(m => m.Reload(), Times.Once);
            disposableMock.Verify(m => m.Dispose(), Times.Once);

            callback?.Invoke(new object());

            configurationRootMock.Verify(m => m.Reload(), Times.Exactly(2));
            disposableMock.Verify(m => m.Dispose(), Times.Exactly(2));

            sut.Dispose();
            disposableMock.Verify(m => m.Dispose(), Times.Exactly(3));
        }
    }
}
