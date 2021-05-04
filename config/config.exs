import Config

config :nostrum,
  token: System.fetch_env!("DISCORD_TOKEN"),
  num_shards: :auto
