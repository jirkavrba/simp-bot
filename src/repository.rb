# frozen_string_literal: true

require "sqlite3"

module SimpBot
  class Repository
    include Boomers::RepositoryMethods
    include Horoscope::RepositoryMethods

    def initialize
      @database = SQLite3::Database.new "simp-bot.db"
      @database.execute <<~SQL
        create table if not exists users (
          user_id     int,
          zodiac_sign varchar(11)           
        )
      SQL
      # TODO: Combine these two statements
      @database.execute <<~SQL
        create table if not exists boomers (
          user_id     int,
          gifs_count  int
        )
      SQL
    end
  end
end
