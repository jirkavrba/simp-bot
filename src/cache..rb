module Astronomia
  class CacheEntry < Struct.new(:value, :inserted_at); end

  class HoroscopeCache
    def initialize
      @storage = {}
    end

    def entry zodiac_sign, &block
      if @storage.has_key? zodiac_sign and is_still_valid? @storage[zodiac_sign]
        @storage[zodiac_sign].value
      else
        value = block.call
        @storage[zodiac_sign] = CacheEntry.new value, Date.today

        value
      end
    end

    private

    def is_still_valid? entry
      entry.inserted_at == Date.today
    end
  end
end