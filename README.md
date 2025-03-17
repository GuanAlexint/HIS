# HIS
医院信息系统
# 🏥 医院挂号病历系统（HIS子系统）

**项目路径**：`main/project01`  
**技术栈**：C# WinForms + SQL Server   

---

## 📋 核心功能  
✅ **患者建档**  
- 登录之后可以建立患者档案卡，并接入患者数据库  
- 可与挂号进行匹配  

✅ **患者挂号**  
- 完成挂号信息填写后上传到患者挂号数据库  
- 选择科室、医师等

✅ **AI诊断**  
- 接入ollama，通过deepseek来创建病历  
- 保存病历，生成检查项目

---

## 🛠️ 开发环境  
```bash
Visual Studio 2022  
.NET Framework 4.8  
SQL Server 2019（需启用Always Encrypted）
