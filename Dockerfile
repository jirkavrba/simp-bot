FROM elixir:1.11

RUN mkdir /app
COPY . /app
WORKDIR /app

RUN mix local.hex --force

CMD ["sh", "/app/run"]