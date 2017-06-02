module.exports = function (grunt) {

    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),

        //concat: {
        //    options: {
        //        separator: ''
        //    },
        //     dist: {
        //         src: ["src/a.js","src/b.js"],
        //         dest: "build/main.js"
        //     }
        // },

        //less编译
        less: {
            oneuiCompile: {
                files: {
                    'css/oneui.css': ['less/framework/main.less']
                }
            },
            unoteCompile: {
                files: {
                    'css/unote.css': ['less/unote/unote.less']
                }
            }
        },

        //css压缩
        cssmin: {
            oneui: {
                files: {
                    'css/unote.min.css': ['css/bootstrap.min.css', 'css/oneui.css', 'css/unote.css']
                }
            }
        },

        //监听
        watch: {
            options: {
                livereload: true
            },
            lessCompress: {
                files: ['*/*.less', '*/*/*.less', '*/*/*/*.less'],
                tasks: ['less','cssmin'],
                options: {
                    spawn: false
                }
            }
        }


        //js编译
        //uglify: {
        //    jstarget1: {
        //        options: {
        //            banner: '/*! <%= pkg.name %> <%= grunt.template.today("yyyy-mm-dd") %> */', //添加注释头
        //            footer: '\n/*! <%= pkg.name %> 最后修改于： <%= grunt.template.today("yyyy-mm-dd") %> */'  //添加注释底
        //        },
        //        files:{
        //            'build/ab.min.js': ['src/a.js', 'src/b.js']
        //        }
        //    },
        //    jstarget2: {
        //        options: {
        //            banner: '/*! 加 乘 */'
        //        },
        //        files: {
        //            'build/ac.min.js': ['src/a.js', 'src/c.js']
        //        }
        //    }
        //}

    });


    //grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-less');
    grunt.loadNpmTasks('grunt-contrib-cssmin');
    grunt.loadNpmTasks('grunt-contrib-watch');

    grunt.registerTask('default', ['less', 'cssmin', 'watch']);

};

