module.exports = {
  lintOnSave: process.env.NODE_ENV !== 'production',
  runtimeCompiler: true,
  devServer: {
    proxy: {
      '^/api': {
        target: 'https://localhost:5001',
        changeOrigin: true
      }
    }
  }
}
