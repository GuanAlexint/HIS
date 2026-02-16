import Vue from 'vue'
import Vuex from 'vuex'
import request from '../request'

Vue.use(Vuex)


export default new Vuex.Store({
  
  /* 全局状态 类似于组件里的 data，
  用于存储应用中需要共享的数据。
  可以在组件中用 this.$store.state.count 来访问它*/
  state: {  
    departmentTreeData: [],                // TODO 部门数据 
    departmentCodeMap: {},
    userInfo: {},

  },      

  /* 同步修改state的方法
  在组件中这样调用:
  this.$store.commit('increment')
  this.$store.commit('setUser', { name: '小明', token: 'abc123' })
  */
  mutations: {  
    setDepartmentTreeData(state,data) {
      state.departmentTreeData = data  
    },
    setUserInfo(state,data) {
      state.userInfo = data
    },
    setDepartmentCodeMap(state,data) {
      state.departmentCodeMap = data
    }
  },  


  /* 用于执行异步操作（如：请求 API），然后通过 commit 调用 mutation 修改状态。
  例：
    async fetchUser({ commit }) {
      const res = await axios.get('/api/user')
      commit('setUser', res.data)
    }
  在组件中调用：
  this.$store.dispatch('fetchUser')*/
  actions: {  
    async getDepartmentTreeData({ commit }) {
      const { data: res } = await request.get('/department/all')
      const data = res.data
      const nodeMap = new Map();
      const codeMap = new Map();
      const tree = [];

      console.log(data)
    
      // 先创建所有节点
      data.forEach(item => {
        nodeMap.set(item.officeId, {
          value: item.officeId,
          label: item.officeName,
          children: []
        });
        codeMap.set(item.officeId,item.officeName)
      });
    
      // 建立父子关系
      data.forEach(item => {
        const id = item.officeId;
        if (id.length === 2) {
          // 一级科室
          tree.push(nodeMap.get(id));
        } else {
          // 找父节点（每两个字符为一级）
          const parentId = id.substring(0, id.length - 2);
          const parent = nodeMap.get(parentId);
          if (parent) {
            parent.children.push(nodeMap.get(id));
          }
        }
      });

      // 递归清理空 children
      function cleanEmptyChildren(nodes) {
        nodes.forEach(node => {
          if (node.children.length > 0) {
            cleanEmptyChildren(node.children); // 递归处理子节点
          } else {
            delete node.children; // 删除空 children
          }
        });
        return nodes
      }

      commit('setDepartmentTreeData',cleanEmptyChildren(tree))
      commit('setDepartmentCodeMap',codeMap)
    
    }
  },  

  /* 用于将 store 分割成模块（module），每个模块有自己的 state、mutations、actions 等，适用于大型项目。 */
  modules: {  

  }     // 模块化管理子 store
})

