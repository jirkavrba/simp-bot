module SimpBot
  module Boomers
    module RepositoryMethods
      def get_gifs_count(user_id)
        gifs_count = @database.execute('select gifs_count from boomers where user_id = ? limit 1', [user_id])

        if gifs_count.empty?
          @database.execute('insert into boomers (user_id, gifs_count) values (?, ?)', [user_id, 0])
          return 0
        end

        gifs_count.flatten.first
      end

      def get_top_ten_boomers
        @database.execute('select user_id, gifs_count from boomers where gifs_count > 0 order by gifs_count desc limit 10')
      end

      def increment_gifs_count(user_id)
        @database.execute('insert or ignore into boomers (user_id, gifs_count) values (?, 0)', [user_id])
        @database.execute('update boomers set gifs_count = (gifs_count + 1) where user_id = ?', [user_id])
      end
    end
  end
end
