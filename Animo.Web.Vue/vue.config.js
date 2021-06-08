const path = require("path");
module.exports = {
  lintOnSave: false,

  chainWebpack: (config) => {
    config.module
      .rule("i18n-resource")
      .test("/.(json5?|ya?ml)$/")
      .include.add(path.resolve(__dirname, "src/locales"))
      .end()
      .type("javascript/auto")
      .use("i18n-resource")
      .loader("@intlify/vue-i18n-loader");
    config.module
      .rule("i18n")
      .resourceQuery(/blocktype=i18n/)
      .type("javascript/auto")
      .use("i18n")
      .loader("@intlify/vue-i18n-loader");
  },

  pluginOptions: {
    i18n: {
      locale: "en",
      fallbackLocale: "en",
      localeDir: "locales",
      enableLegacy: false,
      runtimeOnly: false,
      compositionOnly: false,
      fullInstall: true,
    },
    
    "style-resources-loader": {
      preProcessor: "scss",
      patterns: [path.resolve(__dirname, "./src/assets/styles/*.scss")],
    },
  },
};
