require 'discordrb'
require_relative 'src/setup'

module Astronomia
  bot = Discordrb::Bot.new token: ENV["DISCORD_TOKEN"], ignore_bots: true

  bot.message do |event|
    if event.message.content.start_with? "+horoscope"
      event.respond "Henlo"
    end
  end

  bot.run
end