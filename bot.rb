# frozen_string_literal: true

require "discordrb"
require_relative "src/setup"

module SimpBot
  bot = Discordrb::Bot.new token: ENV["DISCORD_TOKEN"], ignore_bots: true
  repository = SimpBot::Repository.new

  handlers = [
    SimpBot::Animals::MessageHandler.new,
    SimpBot::Facts::MessageHandler.new,

    SimpBot::Boomers::MessageHandler.new(repository),
    SimpBot::Horoscope::MessageHandler.new(repository)
  ]

  bot.message { |event| handlers.each { |handler| handler.handle_message event } }
  bot.message_edit { |event| handlers.each { |handler| handler.handle_message event } }

  bot.run
end
