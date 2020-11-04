# frozen_string_literal: true

require 'sqlite3'

module SimpBot
  class Repository

    include Horoscope::RepositoryMethods

    def initialize
      @database = SQLite3::Database.new 'simp-bot.db'
      @database.execute <<~SQL
        create table if not exists users (
          user_id     int,
          zodiac_sign varchar(11)           
        )
      SQL
    end
  end
end
