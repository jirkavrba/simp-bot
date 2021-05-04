import Config

# This is just to prevent raising exceptions in dyalizer runtime eval
if Mix.env() == :production do
  config :nostrum,
    token: System.fetch_env!("DISCORD_TOKEN"),
    num_shards: :auto
end
