module.exports = {
  "lintOnSave": true,
  "runtimeCompiler": true,

  "pluginOptions": {
    "i18n": {
      "locale": "en-US",
      "fallbackLocale": "en-US",
      "localeDir": "locales"
    }
  },

  "devServer": {
    "watchOptions": {
      "poll": true
    }
  },

  "outputDir": "app",

  "transpileDependencies": [
    "vuetify"
  ],

  pluginOptions: {
    i18n: {
      locale: 'en',
      fallbackLocale: 'en',
      localeDir: 'locales',
      enableInSFC: false
    }
  }
}
