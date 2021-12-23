// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Moq;
using StackExchange.Redis;
using System;
using System.Text.Json;
using Xunit;

namespace Aguacongas.DynamicConfiguration.Redis.Test
{
    public class RedisConfigurationProviderTest
    {
        [Fact]
        public void Constructor_should_verify_arguments()
        {
            Assert.Throws<ArgumentNullException>(() => new RedisConfigurationProvider(null));
        }

        [Fact]
        public void Constructor_should_subscribe_to_redis_channel()
        {
            var subscriberMock = new Mock<ISubscriber>();
            Action<RedisChannel, RedisValue>? callback = null;
            subscriberMock.Setup(m => m.Subscribe(It.IsAny<RedisChannel>(), It.IsAny<Action<RedisChannel, RedisValue>>(), It.IsAny<CommandFlags>()))
                .Callback<RedisChannel, Action<RedisChannel, RedisValue>, CommandFlags>((channel, action, flags) => callback = action);

            var databaseMock = new Mock<IDatabase>();
            databaseMock.Setup(m => m.HashGetAll(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).Returns(new HashEntry[]
            {
                new HashEntry("test", "test"),
                new HashEntry("json", JsonSerializer.Serialize(new object()))
            });
            var connectionMock = new Mock<IConnectionMultiplexer>();
            connectionMock.Setup(m => m.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(databaseMock.Object);
            connectionMock.Setup(m => m.GetSubscriber(It.IsAny<object>())).Returns(subscriberMock.Object);

            var sourceMock = new Mock<IRedisConfigurationSource>();
            sourceMock.SetupGet(m => m.Connection).Returns(connectionMock.Object);
            sourceMock.SetupGet(m => m.Channel).Returns("test");

            var sut = new RedisConfigurationProvider(sourceMock.Object);

            Assert.NotNull(callback);

            callback?.Invoke("test", "test");
        }

        [Fact]
        public void Set_should_replace_values()
        {
            var subscriberMock = new Mock<ISubscriber>();
            subscriberMock.Setup(m => m.Publish(It.IsAny<RedisChannel>(), It.IsAny<RedisValue>(), It.IsAny<CommandFlags>())).Verifiable();

            var databaseMock = new Mock<IDatabase>();
            databaseMock.Setup(m => m.HashGet(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<CommandFlags>())).Returns(JsonSerializer.Serialize(new RedisConfigurationOptions()));
            databaseMock.Setup(m => m.HashDelete(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<CommandFlags>())).Verifiable();

            var connectionMock = new Mock<IConnectionMultiplexer>();
            connectionMock.Setup(m => m.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(databaseMock.Object);
            connectionMock.Setup(m => m.GetSubscriber(It.IsAny<object>())).Returns(subscriberMock.Object);

            var sourceMock = new Mock<IRedisConfigurationSource>();
            sourceMock.SetupGet(m => m.Connection).Returns(connectionMock.Object);
            sourceMock.SetupGet(m => m.Channel).Returns("test");

            var sut = new RedisConfigurationProvider(sourceMock.Object);

            sut.Set("json", JsonSerializer.Serialize(new RedisConfigurationOptions()));

            databaseMock.Verify();
            subscriberMock.Verify();

            databaseMock.Setup(m => m.HashGet(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<CommandFlags>())).Returns(RedisValue.Null);
            sut.Set("json", JsonSerializer.Serialize(new RedisConfigurationOptions()));
        }
    }
}