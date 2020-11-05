# frozen_string_literal: true

require 'discordrb'
require_relative 'src/setup'

module SimpBot
  bot = Discordrb::Bot.new token: ENV['DISCORD_TOKEN'], ignore_bots: true
  repository = SimpBot::Repository.new

  handlers = [
    SimpBot::Animals::MessageHandler.new(repository),
    SimpBot::Boomers::MessageHandler.new(repository),
    SimpBot::Facts::MessageHandler.new(repository),
    SimpBot::Horoscope::MessageHandler.new(repository)
  ]

  bot.message do |event| handlers.each { |handler| handler.handle_message event } end
  bot.message_edit do |event| handlers.each { |handler| handler.handle_message event } end


  bot.run
end
