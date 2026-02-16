<template>
<div class="pati-visit-container">

<el-form ref="form" :model="visitInfo" label-width="80px">
    <el-form-item label="患者姓名">
        <span>{{pname}}</span>
    </el-form-item>

    <el-form-item label="挂号科室">
        <el-cascader
            v-model="depart_code"
            :options="departmentTreeData"
            :props="{ expandTrigger: 'hover' }"
            @change="departmentChange"></el-cascader>

    </el-form-item>

    <el-form-item label="挂号类型">
        <el-select v-model="visitInfo.rtype" @change="onVisitTypeChange">
            <el-option label="普通" value="普通"></el-option>
            <el-option label="专家" value="专家"></el-option>
        </el-select>
    </el-form-item>

    <el-form-item label="医生姓名">
        <el-select v-model="visitInfo.doctName" >
            <el-option 
                v-for="item in doctorNames_filtered" 
                :key = "item.userId"
                :label="item.userName" 
                :value="item.userName"></el-option>
        </el-select>
    </el-form-item>

    
    <el-form-item prop="pbirthday" label="挂号日期">
        <el-date-picker 
            type="date" 
            placeholder="选择日期" 
            v-model="visitInfo.registDate" 
            format="yyyy 年 M 月 d 日"
            value-format="yyyy-MM-dd"
            ></el-date-picker>
    </el-form-item>
    
    <el-form-item>
        <el-button type="primary" @click="onSubmit">挂号</el-button>
        <el-button @click="onCancel">取消</el-button>
    </el-form-item>
 
</el-form>
</div>
</template>



<script>
// import { mapState, mapMutations, mapActions } from '../../node_modules__/vuex/dist/vuex.mjs'
import { mapState, mapMutations, mapActions } from 'vuex'


export default {
    name: 'PushPatiVisit',
    props: {
        pid: {
            type: String,
            // require: true,
            default: '',
        },
        pname: {
            type: String,
            require: true,
            default: ''
        }
    },
    data() {
        return {
            doctorNames: [],
            doctorNames_filtered: [],
            depart_code: [],
            visitInfo: {
                rid: '',
                pid: '',
                rtype: '',
                dpmtnNme: '',
                doctName: '',
                registDate: ''
            }
        }
    },
    created() {
        window.patiVisitV = this
    },
    mounted() {
        this.getAllDoctor()
    },
    methods: {
        departmentChange(node) {
            console.log(node)
            const code_len = node.length
            if (code_len>1) {
                
                this.visitInfo.dpmtnNme = this.departmentCodeMap.get(node[code_len-1])
                // this.visitInfo.rtype = this.departmentCodeMap.get(node[0])

            }
        },
        // 获取所有医生信息
        async getAllDoctor() {
            const {data: res} = await this.$http.get('/user/all')
            this.doctorNames = res.data
        },

        onVisitTypeChange(){
            this.doctorNames_filtered = _.filter(this.doctorNames,d=>{return d.docType===this.visitInfo.rtype})
        },

        // 挂号接口
        async onSubmit() {
            this.visitInfo.pid = this.pid
            this.visitInfo.rid = formatDateTimeToYYMMDDHHMMSS()
            const {data: res} = await this.$http.post('/outvisit',this.visitInfo)
            if (res.code===200) {
                this.$message.success(this.pname+' 挂号成功')
                this.$emit('vist_res',res)
            }else{
                this.$message.error(this.pname+' 挂号失败：'+res.message)
                this.$emit('visit_res','')
            }
            this.$emit('visit_res','') // TODO 奇怪，只有这里有用？
        },
        onCancel() {
            this.$emit('visit_res','cancel')
        }
    },
    computed: {
        ...mapState(['departmentTreeData','departmentCodeMap']),
    },

}
</script>

<style scoped>
.pati-visit-container {
    width: 100%;
}
</style>
