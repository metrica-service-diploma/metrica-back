TODO:

- [ ] Добавить безопасный проброс секретов
- [ ] Добавить эффективный сидинг данными БД

```
docker run -d --name clickhouse-server -p 8123:8123 -p 9000:9000 -p 9009:9009 -p 9440:9440 -e CLICKHOUSE_USER=default -e CLICKHOUSE_PASSWORD=password123 -e CLICKHOUSE_DEFAULT_ACCESS_MANAGEMENT=1 clickhouse/clickhouse-server:latest
```
