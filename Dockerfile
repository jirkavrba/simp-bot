FROM elixir:1.11

RUN mkdir /app
COPY . /app
WORKDIR /app

RUN mix local.hex --force
RUN mix deps.get

CMD ["sh", "/app/run"]