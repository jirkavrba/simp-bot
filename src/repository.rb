require 'sqlite3'

module Astronomia
  class Repository
    def initialize
      @database = SQLite3::Database.new 'astronomia.db'
      @database.execute <<~SQL
        create table if not exists users (
          user_id     int,
          zodiac_sign varchar(11)           
        )
      SQL
    end

    def find_user_zodiac_sign user_id
      zodiac_sign = @database.execute("select zodiac_sign from users where user_id = ? limit 1", [user_id])

      raise Errors::UserZodiacMissingError if zodiac_sign.empty?

      # It's wrapped in [[ "sign" ]] due to sqlite library design
      zodiac_sign.flatten.first
    end

    def find_users_by_zodiac_sign zodiac_sign
      @database.execute("select user_id from users where zodiac_sign = ? limit 1", [zodiac_sign]).flatten
    end

    def register_user user_id, zodiac_sign
      begin
        find_user_zodiac_sign user_id
      rescue Errors::UserZodiacMissingError
        @database.execute("insert into users (user_id, zodiac_sign) values (?, ?)", [user_id, zodiac_sign])
      else
        @database.execute("update users set zodiac_sign = ? where user_id = ?", [zodiac_sign, user_id])
      end
    end
  end
end