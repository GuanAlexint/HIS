<template>
    <el-container class="home-container">
        <!-- 头部区域 -->
        <el-header class="header-container">
            <div style="display:flex;gap:10px;align-items: center;">
            <div class="header-sys-icon">
                <i class="iconfont icon-HIS"></i>
                <span class="span-sys-name">医院电子信息系统</span>            
            </div>
            <el-button  @click="goHome" icon="el-icon-s-home" class="button-home" size=mini>主页</el-button>
            </div>

            <span class="span-role-name">{{path_map_role[$route.fullPath]}}</span>
            <div style="display:flex;">
                <el-button size="small" class="user-info-but"   type="warning"  @click="showDoctorInfo">
                    <i class="iconfont icon-yisheng2"/>
                    <span>{{userInfo.userName}}</span>
                    </el-button>
                <el-button type="info" @click="logout" size=mini>退出</el-button>
            </div>
        </el-header>
        <!-- 页面主体区域 -->
        <el-container class="el-container-class">
            <!-- 侧边栏 -->
            <el-aside :width="isCollapse ? '64px' : '160px'">
                <div class="toggle-button" @click="toggleCollapse">|||</div>
                <!-- 侧边栏菜单区域 -->
                <el-menu background-color="#333744" text-color="#fff" active-text-color="#409EFF" :unique-opened="false"
                         
                         :collapse="isCollapse" :collapse-transition="false" router 
                         :default-active="activePath">
                    

                    <el-menu-item index="/patientInfo" key="view_all">
                        <i class="iconfont icon-huanzheguanli"></i>
                        <span>患者建档信息</span>
                    </el-menu-item>


                    <el-submenu index="" :key="777">
                        <template slot="title">
                            <i class="iconfont icon-qingbingliguanli"></i>
                            <span>患者就诊</span>
                        </template>

                        <el-menu-item index="/medicalRecord" :key="779">
                            <template slot="title">
                                <i class="iconfont icon-jiuyijiuzhen"></i>
                                <span>诊断与病历</span>
                            </template>
                        </el-menu-item>

                        <el-menu-item index="/medicalOrder" :key="780">
                            <template slot="title">
                                <i class="iconfont icon-yizhufenxi"></i>
                                <span>医嘱</span>
                            </template>
                        </el-menu-item>

                    </el-submenu>

                    <el-menu-item index="/medicalBill" :key="778">
                        <i class="iconfont icon-jiesuan" ></i>
                        <span>结算</span>
                    </el-menu-item>


                    

                </el-menu>
            </el-aside>
            <!-- 右侧内容主体 -->
            <el-main id="body-main">
                <!-- 路由占位符 -->
                <keep-alive >
                    <router-view :key="$route.fullPath"></router-view>
                </keep-alive>
            </el-main>
        </el-container>

        <!-- 医生信息对话框 -->
        <el-dialog title="医生信息" :visible.sync="doctorInfoDialogVisible" width="30%">
            <el-form :model="doctorInfo" label-width="80px">
                <el-form-item label="姓名">
                    <el-input v-model="doctorInfo.userName" disabled></el-input>
                </el-form-item>
                <el-form-item label="邮箱">
                    <el-input v-model="doctorInfo.email" disabled></el-input>
                </el-form-item>
                <el-form-item label="医生类型">
                    <el-input v-model="doctorInfo.docType" disabled></el-input>
                </el-form-item>
            </el-form>
            <span slot="footer" class="dialog-footer">
                <el-button @click="doctorInfoDialogVisible = false">关闭</el-button>
            </span>
        </el-dialog>

    </el-container>
</template>

<script>
    // import { mapState, mapMutations, mapActions } from '../../node_modules__/vuex/dist/vuex.mjs'
    import { mapState, mapMutations, mapActions } from 'vuex'

    export default {
        data() {
            return {
                
                // 是否折叠
                isCollapse: true,
                // 被激活的链接地址
                activePath: '/patientInfo',
                path_map_role: {
                    '/home': '主页',
                    '/patientInfo': '患者信息',
                    '/medicalRecord': '诊断与病历',
                    '/medicalOrder': '医嘱开立与缴费'
                },
                // 医生信息对话框是否可见
                doctorInfoDialogVisible: false,
                // 医生详细信息
                doctorInfo: {
                    userName: '',
                    email: '',
                    doctype: ''
                }
            }
        },

        created() {
            window.homeV = this
            this.getDepartmentTreeData()

        },
        mounted() {
            this.getStatisticData()
            setInterval(this.getStatisticData,50000)
        },
        methods: {
            ...mapActions(['getDepartmentTreeData']),
            logout() {
                window.sessionStorage.clear()
                this.$router.push('/login')
            },
            goHome(){
                this.$router.push('/overview')
            },
            
            // 点击按钮，切换菜单的折叠与展开
            toggleCollapse() {
                this.isCollapse = !this.isCollapse
            },
            // 保存链接的激活状态
            saveNavState(activePath) {
                window.sessionStorage.setItem('activePath', activePath)
                this.activePath = activePath
            },
            // 显示医生信息
            async showDoctorInfo() {
                try {
                    // 获取当前登录的医生ID
                    console.log(this.userInfo)
                    const userId = this.userInfo.userId;
                    // 从服务器获取医生详细信息
                    const { data: res } = await this.$http.get(`user/${userId}`);
                    
                    if (res.code !== 200) {
                        return this.$message.error('获取医生信息失败');
                    }
                    
                    // 更新医生信息
                    this.doctorInfo = res.data;
                    // 显示对话框
                    this.doctorInfoDialogVisible = true;
                } catch (error) {
                    console.error('获取医生信息出错：', error);
                    this.$message.error('获取医生信息失败，请稍后再试');
                }
            }
        },
        computed: {
            ...mapState(['departmentTreeData','userInfo']),
        },
        watch: {
            
        }
    }
</script>

<style scoped>
    .home-container {
        height: 100%;
    }

    .header-container {
        background-color: #333744;
        display: flex;
        justify-content: space-between;
        padding-left: 0;
        align-items: center;
        color: #fff;
        font-size: 20px;
        height: 40px !important;

    }

    .header-sys-icon {
        display: flex;
        justify-content: space-between;
    }

    .header-sys-icon .span-sys-name {
        font-size: 16px;
        font-family: KaiTi;
        padding-left: 5px;
    }


    .span-role-name {
        font-size: 18px;
    }


    .el-aside {
        background-color: #333744;
    }

    .el-menu {
        border-right: none;
    }
    .el-menu-item {
        min-width: 160px;
    }

    .el-main {
        background-color: #0aedf1;
    }

    .toggle-button {
        background-color: #4a5064;
        font-size: 10px;
        line-height: 24px;
        color: #fff;
        text-align: center;
        letter-spacing: 0.2em;
        cursor: pointer;
    }

    #body-main {
        background-color: #fff !important;
        padding: 0px;
        margin: 10px;
    }
    .el-container-class {
        height: calc(100% - 30px);
    }


    .el-menu .iconfont {
        font-size: 20px;
        padding-right: 4px;
    }

    .icon-qingbingliguanli {
        font-size: 26px !important;
    }

    .icon-jiuyijiuzhen {
        font-size: 23px !important;
    }

    /* 添加医生信息按钮样式 */
    .user-info-but {
        cursor: pointer;
    }

    
    .card-container {
        display: flex;
        flex-wrap: wrap;
        gap: 20px;
    }


  .clearfix:before,
  .clearfix:after {
    display: table;
    content: "";
  }
  .clearfix:after {
    clear: both
  }

  .box-card {
    width: 560px;
    height: 450px;
  }





.over-view {
    padding-top: 20px;
    width: 100%;
    height: 100px;
    display: flex;
}

.el-statistic >>> .head{
    font-size: 18px !important;
}
    
.el-statistic >>> .con .number {
    font-size: 25px !important;
}


.button-home {
    background-color: transparent;
    color: white;
    border: 0;
}

    
</style>
