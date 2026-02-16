<template>
    <div class="mobile-container">
    <header class="header">📋 费用支付</header>
    <main class="content">
      <div class="order-list">
        <div class="order-item" v-for="item in orderInfos" :key="item.orderid">
            {{ item.ordertype }}：{{ item.ordername }} {{ item.totalOrder }} × {{ item.orderprice }} 总计：{{ item.orderallprice }}元
        </div>

        <div class="total-price">
      💰     合计总费用：{{ totalPrice }} 元
        </div>

      </div>

      <el-button class="btn" @click="outPay" :disabled="orderInfos.length===0">确认支付</el-button>
    </main>
    <footer class="footer">© 2025 His App</footer>
  </div>
</template>

<script>

export default {
    data() {
        return {
            rid: '',
            selectedOrders: [],
            orderInfos: []
        }
    },
    mounted() {
        window.title = '费用支付'
        window.outPayV=this
        this.selectedOrders = this.$route.query.selectedOrders.split(',') 
        this.rid = this.$route.query.rid
        this.getOrderToPayInfo()
    },
    methods: {
        
        async getOrderToPayInfo() {
            const {data:res} = await this.$http.get('/doctororder/rid/'+this.rid)
            if(res.code===200){
                this.$message.success('成功获取医嘱信息！')
                this.orderInfos = _.filter(res.data,o=>{ return !o.ispaid && this.selectedOrders.indexOf(''+o.orderid)>-1 })
            }else {
                this.$message.error('获取医嘱信息失败！请联系xxx: 15722222222')
            }

        },
        outPay() {
            console.log('orderInfos',this.orderInfos)
            let not_ok_order = []
            this.orderInfos.forEach(async o=>{
                o.ispaid=true
                const {data:res} =await this.$http.put('/doctororder',o)
                if(res.code !== 200){ not_ok_order.push(o) }
            })
            this.orderInfos = not_ok_order
        }
    },
    computed: {
        totalPrice() {
            return this.orderInfos.reduce((sum, item) => sum + (item.orderallprice || 0), 0);
        }
    }
}
</script>

<style scoped>
/* 移动端响应式布局 */
.mobile-container {
  display: flex;
  flex-direction: column;
  height: 100vh;
  font-family: 'Helvetica Neue', sans-serif;
  background-color: #f7f7f7;
  padding: 1rem;
  box-sizing: border-box;
}

.header {
  font-size: 1.4rem;
  font-weight: bold;
  text-align: center;
  padding: 0.5rem 0;
  background-color: #4caf50;
  color: white;
  border-radius: 0.5rem;
}

.content {
  flex: 1;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  padding: 1rem;
}

.text {
  font-size: 1.2rem;
  margin-bottom: 1rem;
}

.btn {
  font-size: 1rem;
  padding: 0.6rem 1.2rem;
  background-color: #2196f3;
  color: white;
  border: none;
  border-radius: 0.4rem;
  cursor: pointer;
}

.btn:active {
  background-color: #1976d2;
}

.footer {
  text-align: center;
  padding: 0.8rem;
  font-size: 0.9rem;
  color: #777;
}

.order-list {
  padding: 1rem;
  font-size: 16px;
}
.order-item {
  margin-bottom: 0.5rem;
  border-bottom: 1px dashed #ccc;
  padding-bottom: 0.5rem;
}
.total-price {
  margin-top: 1rem;
  font-weight: bold;
  font-size: 18px;
  color: #2c3e50;
}
</style>