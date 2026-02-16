<template>
  <div>
    <div class="patient-head">
        <el-input 
            class="patient-search-input" 
            placeholder="患者搜索（姓名/身份证/电话）" 
            v-model="patient_search_str"
            @change="searchByKeyword"
            clearable>
            <el-button
                @click="searchByKeyword"
                slot="append" icon="el-icon-search"></el-button>
        </el-input>
        <div class="patient-add-div">
            <el-button 
                @click="editOrAddPatient('add','')"
                type="primary" icon="el-icon-plus" size="mini" circle></el-button>
        </div>
    </div>

    <div class="data-table-show">
        <el-table
            :data="patientBaseInfosShow"
            class="patient-table"
            border
            height="600"
        >
            <el-table-column
                label="姓名"
                prop="pname"
                width="80"
            >
            </el-table-column>
            <el-table-column
                label="性别"
                prop="pgender"
                width="50"
            ></el-table-column>
            <el-table-column
                label="手机号"
                prop="ptel"
                width="100"
            ></el-table-column>
            <el-table-column
                label="身份证"
                prop="pidcard"
                width="200"
            ></el-table-column>
            

            <el-table-column
                label="操作"
                width="90">
                <template slot-scope="props">
                    <div class="operate-column-container">
                        <i @click="editOrAddPatient('edit',props.row)"
                            title="编辑"
                            class="el-icon-edit"/>
                        <i @click="deletePatient(props.row)"
                           title="删除"
                           class="el-icon-delete">
                        </i>
                        <i @click="pushPatientToOutVisit(props.row)"
                           title="为此患者挂号"
                           class="el-icon-first-aid-kit">
                        </i>
                        
                    </div>
                </template>
            </el-table-column>

        </el-table>
        <div class="page-container">
            <el-pagination
                @size-change="handleSizeChange"
                @current-change="handleCurrentChange"
                :current-page="page_ctr.current_page"
                :page-size="page_ctr.page_size"
                layout="total, sizes, prev, pager, next, jumper"
                :total="patientBaseInfos.length">
            </el-pagination>
        </div>

    </div>

    <!-- 患者信息编辑/添加 对话框******************************************************************             -->
    <el-dialog :title="patientInfoCtr.title" :visible.sync="patientInfoCtr.dialogVisible" width="70%" > <!-- @close="addDialogClosed" -->
      <!-- 内容主体区域 -->
        <el-form :model="patientInfo" :rules="patientInfoRules" ref="patientInfo" label-width="100px" class="demo-patientInfo">
            <el-form-item label="姓名" prop="pname" required>
                <el-input v-model="patientInfo.pname"></el-input>
            </el-form-item>
            
            <el-form-item label="性别" prop="pgender" required>
                <el-radio-group v-model="patientInfo.pgender">
                    <el-radio label="男"></el-radio>
                    <el-radio label="女"></el-radio>
                </el-radio-group>
            </el-form-item>

            <el-form-item label="出生日期" >
                <el-form-item prop="pbirthday">
                    <el-date-picker 
                        type="date" 
                        placeholder="选择日期" 
                        v-model="patientInfo.pbirthday" 
                        format="yyyy 年 M 月 d 日"
                        value-format="yyyy-MM-dd"
                        style="width: 100%;"></el-date-picker>
                </el-form-item>
            </el-form-item>

            <el-form-item label="身份证" prop="pidcard" required>
                <el-input v-model="patientInfo.pidcard"></el-input>
            </el-form-item>

            <el-form-item label="手机号" prop="ptel" >
                <el-input v-model="patientInfo.ptel"></el-input>
            </el-form-item>
            
        </el-form>

        <!-- 底部区域 -->
        <span slot="footer" class="dialog-footer">
            <el-button @click="patientInfoCtr.dialogVisible = false">取 消</el-button>
            <el-button type="primary" @click="savePatientInfo">保 存</el-button>
        </span>
    </el-dialog>


    <!-- TODO key的作用 -->
    <el-dialog title="患者挂号" :visible.sync="patiToVist.dialogVisible" width="70%" >
        <pati-visit 
            :key="patiToVist.pid" 
            :pid="patiToVist.pid" :pname="patiToVist.pname" @visit_res="handleVisitClose"/>

        
    </el-dialog>


  </div>
</template>

<script>
import PushPatiVisit from '@/components/PushPatiVisit.vue'
export default {
    components: {
        'pati-visit': PushPatiVisit
    },
    data() {
        return {
            patient_search_str: '',
            patientBaseInfos: [],
            patientBaseInfosShow: [],

            // 患者信息 编辑/新增 控制
            patientInfo: {
                pname: '',
                pgender: '男',
                pbirthday: '1990-01-01',
                pidcard: '',
                ptel: ''
            }, 
            patientInfoCtr: {
                title: '新患者建档',
                dialogVisible: false,
                type: 'add'
            },
            patientInfoRules: {

            },
            // 分页控制
            page_ctr:{
                page_size:10,
                current_page:1
            },


            // 挂号
            patiToVist: {
                pid: '',
                pname: '',
                dialogVisible: false
            }
        }
    },

    created() {
        window.patientV = this

    },
    methods: {
        async searchByKeyword() {
            const {data: res} = await this.$http.get('/patient/keyword/' + this.patient_search_str)
            console.log(res)

            if (res.code === 200) {
                this.patientBaseInfos = res.data
                this.handleCurrentChange(1) // 重置页面为1
                this.$message.success(res.message)
            }else{
                this.$message.error(res.message)
            }

        },

        // 编辑患者信息
        editOrAddPatient(type,row) {

            this.patientInfoCtr.type = type
            if (type === 'edit') {
                this.patientInfo = row // TODO 编辑的弹框消失后应该展示真实的患者数据
                this.patientInfoCtr.title = "编辑患者信息"
                this.patientInfoCtr.dialogVisible = true
            
            }else if(type === 'add' ) {
                this.patientInfoCtr.title = "新增患者信息"
                this.patientInfoCtr.dialogVisible = true
                this.patientInfo = {
                    pid: formatDateTimeToYYMMDDHHMMSS(),
                    pname: '',
                    pgender: '男',
                    pbirthday: '1990-01-01',
                    pidcard: '',
                    ptel: ''
                }
            }
            console.log(row)
        },
        // 删除患者信息
        deletePatient(row) {
            console.log(row)
        },

        async savePatientInfo() {
            this.patientInfoCtr.dialogVisible = false
            if (this.patientInfoCtr.type === 'edit') {
                const {data: res} = await this.$http.put('/patient',this.patientInfo)
                console.log(res)
                if (res.code===200) {
                    this.$message.success(this.patientInfo.pname+' 信息更新成功')
                }else {
                    this.$message.error(this.patientInfo.pname+' 信息更新失败')
                }
            }else{
                const {data: res} = await this.$http.post('/patient',this.patientInfo)
                console.log(res)
                if (res.code===200) {
                    this.$message.success(this.patientInfo.pname+' 信息添加成功')
                }else {
                    this.$message.error(this.patientInfo.pname+' 信息添加失败')

                }
            }
        },


        pushPatientToOutVisit(row) {
            console.log(row)
            this.patiToVist.pid = row.pid
            this.patiToVist.pname = row.pname
            this.patiToVist.dialogVisible = true
        },


        // 分页操作后重新刷新Table
        handleSizeChange(page_size){
            this.page_ctr.page_size=page_size
            this.patientBaseInfosShow = this.patientBaseInfos.slice((this.page_ctr.current_page-1)*this.page_ctr.page_size,this.page_ctr.current_page*this.page_ctr.page_size)
        },
        handleCurrentChange(current_page){
            this.page_ctr.current_page = current_page
            this.patientBaseInfosShow = this.patientBaseInfos.slice((this.page_ctr.current_page-1)*this.page_ctr.page_size,this.page_ctr.current_page*this.page_ctr.page_size)
        },
        handleVisitClose() {
            this.patiToVist.dialogVisible=false
            console.log('close')
        }
    }
}
</script>


<style scoped>
.patient-table {
    height: 400px !important;
    width: 600px;
}
.patient-head {
    width: 550px;
    display: flex; 
    justify-content: space-between; 
}
.patient-head  .el-input{
    width: 500px;
}
.patient-head  .patient-add-div{
    display: flex; align-items: center; /* 子元素div->button 垂直居中 */
}
.operate-column-container {
    display: flex;
    justify-content: space-between;
}
.operate-column-container i {
    font-size: 15px;
    cursor: pointer;
}
</style>
