# frozen_string_literal: true

require 'discordrb'
require_relative 'src/setup'

module SimpBot
  bot = Discordrb::Bot.new token: ENV['DISCORD_TOKEN'], ignore_bots: true
  repository = SimpBot::Repository.new

  handlers = [
      SimpBot::Horoscope::MessageHandler.new(repository)
  ]

  bot.message do |event|
    handlers.each do |handler| handler.handle_message event end
  end

  bot.run
end
