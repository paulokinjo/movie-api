// babel.config.js
module.exports = {
    presets: [
        ['@babel/preset-env', { targets: { node: 'current' } }], // Important for Node tests
        '@babel/preset-typescript',
        "@babel/preset-react",
    ],
    plugins: [
        // any babel plugins you may need
    ]
};