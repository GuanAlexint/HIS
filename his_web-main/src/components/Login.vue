<template>
  <div class="login_container">
    <div class="login_box">
      <!-- 头像区域 -->
      <div class="avatar_box">
        <img src="../assets/his.png" alt="">
      </div>
      <!-- 登录表单区域 -->
      <el-form ref="loginFormRef" :model="loginForm" :rules="loginFormRules" label-width="0px" class="login_form">
        <!-- 用户名 -->
        <el-form-item prop="username" >
          <el-input v-model="loginForm.username" prefix-icon="iconfont icon-user"></el-input>
        </el-form-item>
        <!-- 密码 -->
        <el-form-item prop="password" >
          <el-input v-show="!password_show_flag" v-model="loginForm.password" type="password">
            <i slot="prefix" class="iconfont icon-eye-close" @click="togglePasswordVisibility"></i>
          </el-input>
          <el-input v-show="password_show_flag" v-model="loginForm.password">
            <i slot="prefix" class="iconfont icon-eye" @click="togglePasswordVisibility"></i>
          </el-input>
        </el-form-item>
        <!-- 按钮区域 -->
        <el-form-item class="btns">
          <el-button type="primary" @click="login">登录</el-button>
          <el-button type="info" @click="resetLoginForm">重置</el-button>
        </el-form-item>
      </el-form>
    </div>
  </div>
</template>

<script>
// import { mapState, mapMutations, mapActions } from '../../node_modules__/vuex/dist/vuex.mjs'
import { mapState, mapMutations, mapActions } from 'vuex'


export default {
  data() {
    return {
      // 密码框里密码是否显示
      password_show_flag: false,

      // 这是登录表单的数据绑定对象
      loginForm: {
        username: '',
        password: ''
      },
      // 这是表单的验证规则对象
      loginFormRules: {
        // 验证用户名是否合法
        username: [
          { required: true, message: '请输入登录名称', trigger: 'blur' },
          { min: 2, max: 10, message: '长度在 2 到 10 个字符', trigger: 'blur' }
        ],
        // 验证密码是否合法
        password: [
          { required: true, message: '请输入登录密码', trigger: 'blur' },
          { min: 3, max: 15, message: '长度在 3 到 15 个字符', trigger: 'blur' }
        ]
      }
    }
  },
  mounted() {
      window.loginV = this
  },
  methods: {
    ...mapMutations(['setUserInfo']),
    //切换密码可见性
    togglePasswordVisibility(){
      this.password_show_flag = !this.password_show_flag;
    },
    // 点击重置按钮，重置登录表单
    resetLoginForm() {
      // console.log(this);
      this.$refs.loginFormRef.resetFields()
    },
    async login() {
      this.$refs.loginFormRef.validate(async valid => {
        if (!valid) return
        const { data: res } = await this.$http.post('user/login', this.loginForm)

        console.log(res)

        if (res.code !== 200) return this.$message.error(res.message)
        this.$message.success('登录成功')

        // 1. 将登录成功之后的 token，保存到客户端的 sessionStorage 中
        //   1.1 项目中出了登录之外的其他API接口，必须在登录之后才能访问
        //   1.2 token 只应在当前网站打开期间生效，所以将 token 保存在 sessionStorage 中
        window.sessionStorage.setItem('token', res.data.token)
        this.setUserInfo(res.data.user)
        // 2. 通过编程式导航跳转到后台主页，路由地址是 /overview
        this.$router.push('/overview')
      })
    }
  }
}
</script>

<style>
.login_container {
  background-color: #2b4b6b;
  height: 100%;
}

.login_box {
  width: 450px;
  height: 300px;
  background-color: #fff;
  border-radius: 3px;
  position: absolute;
  left: 50%;
  top: 50%;
  transform: translate(-50%, -50%);
}

.avatar_box {
  height: 130px;
  width: 130px;
  border: 1px solid #eee;
  border-radius: 10%;
  padding: 10px;
  box-shadow: 0 0 10px #ddd;
  position: absolute;
  left: 50%;
  transform: translate(-50%, -50%);
  background-color: #fff;
}

.avatar_box img {
  width: 100%;
  height: 100%;
  border-radius: 10%;
  background-color: #eee;
}

.login_form {
  position: absolute;
  bottom: 0;
  width: 100%;
  padding: 0 20px;
  box-sizing: border-box;
}

.btns {
  display: flex;
  justify-content: flex-end;
}

.password-container {
  position: relative;
}

.password-icon {
  position: absolute;
  top: 0;
  right: 10px;
  height: 100%;
  display: flex;
  align-items: center;
  cursor: pointer;
  z-index: 1;
}

</style>
