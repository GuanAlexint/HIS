# his_web
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
医院信息系统实现web页面
=======
=======
>>>>>>> f07ebe9c01ec85ce85a91dbff80c15d5964d73dd
=======
>>>>>>> b65eb77edd33d23bbfd25f1254602c5f3e470f98

## Project setup
```
npm install
```

### Compiles and hot-reloads for development
```
npm run serve
```

### Compiles and minifies for production
```
npm run build
```

### Customize configuration
See [Configuration Reference](https://cli.vuejs.org/config/).
<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> f07ebe9 (er)
=======
>>>>>>> f07ebe9c01ec85ce85a91dbff80c15d5964d73dd


## 数据库表初始化
### init
CREATE TABLE `pati_out_visit` (
  `Rid` varchar(15) NOT NULL COMMENT '挂号编码',
  `Pid` varchar(20) DEFAULT NULL COMMENT '病人编号',
  `Rtype` varchar(45) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL COMMENT '挂号类型',
  `DpmtnNme` varchar(45) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL COMMENT '科室名称',
  `DoctName` varchar(45) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL COMMENT '医生姓名',
  `RegistDate` varchar(45) DEFAULT NULL COMMENT '就诊日期',
  PRIMARY KEY (`Rid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='患者挂号表'
;

CREATE TABLE `pati_info_basic` (
  `Pid` varchar(15) NOT NULL COMMENT '病人编号',
  `Pname` varchar(45) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL COMMENT '姓名',
  `Pgender` varchar(45) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL COMMENT '性别',
  `Pidcard` varchar(45) DEFAULT NULL COMMENT '身份证',
  `Pbirthday` date DEFAULT NULL COMMENT '出生日期',
  `Ptel` varchar(45) DEFAULT NULL COMMENT '电话',
  PRIMARY KEY (`Pid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='患者信息表'
;

CREATE TABLE `medicalrecord` (
  `id` varchar(15) NOT NULL COMMENT '门诊病历号',
  `Rid` varchar(15) NOT NULL COMMENT '挂号编号',
  `patiid` varchar(15) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL COMMENT '就诊卡号',
  `patiname` varchar(45) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL COMMENT '患者姓名',
  `medical` varchar(3000) DEFAULT NULL COMMENT '病历内容',
  `Appointmenttime` date DEFAULT NULL COMMENT '就诊时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='患者病历表'
;

CREATE TABLE `hospitaldepartment` (
  `officeid` varchar(10) NOT NULL,
  `officename` varchar(45) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `officetype` varchar(45) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  PRIMARY KEY (`officeid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3

;

CREATE TABLE `doctororder_dict` (
  `ordid` VARCHAR(20) NOT NULL COMMENT '医嘱项主键ID（字符串）',
  `ordername` VARCHAR(100) NOT NULL COMMENT '医嘱名称',
  `ordertype` VARCHAR(50) NOT NULL COMMENT '医嘱类型（药品/检查/治疗等）',
  `inputstr` VARCHAR(50) NOT NULL COMMENT '医嘱名称首字母（中文首字母大写）',
  `drugSpecifications` VARCHAR(100) DEFAULT NULL COMMENT '药品规格',
  `takeMedicine` VARCHAR(50) DEFAULT NULL COMMENT '给药途径',
  `Frequency` VARCHAR(50) DEFAULT NULL COMMENT '频次',
  `price` FLOAT DEFAULT NULL COMMENT '单价',
  PRIMARY KEY (`ordid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='医嘱字典表';

-- 药品类
INSERT INTO doctororder_dict (ordid, ordername, ordertype, inputstr, drugSpecifications, takeMedicine, Frequency, price) VALUES
('ORD006', '左氧氟沙星片', '药品', 'ZYFSXP', '0.1g*10片/盒', '口服', '每日一次', 3.2),
('ORD007', '氯化钠注射液', '药品', 'LHNZSY', '500ml/瓶', '静脉滴注', '一次', 5.0),
('ORD008', '葡萄糖注射液', '药品', 'PTTZSY', '250ml/瓶', '静脉滴注', '一次', 4.5),
('ORD009', '复方甘草片', '药品', 'FFGCP', '100片/瓶', '口服', '每日三次', 1.8),
('ORD010', '维生素C片', '药品', 'WSSC P', '100mg*100片/瓶', '口服', '每日一次', 2.0);

-- 检查类
INSERT INTO doctororder_dict (ordid, ordername, ordertype, inputstr, drugSpecifications, takeMedicine, Frequency, price) VALUES
('ORD011', '腹部彩超', '检查', 'FBCC', NULL, NULL, NULL, 200.0),
('ORD012', '血常规检查', '检查', 'XCGJC', NULL, NULL, NULL, 50.0),
('ORD013', '尿常规检查', '检查', 'NCGJC', NULL, NULL, NULL, 30.0),
('ORD014', '心电图检查', '检查', 'XDTJC', NULL, NULL, NULL, 100.0),
('ORD015', '核磁共振检查', '检查', 'HCGZJC', NULL, NULL, NULL, 800.0);

-- 治疗类
INSERT INTO doctororder_dict (ordid, ordername, ordertype, inputstr, drugSpecifications, takeMedicine, Frequency, price) VALUES
('ORD016', '针灸治疗', '治疗', 'ZJZL', NULL, NULL, '每日一次', 60.0),
('ORD017', '推拿治疗', '治疗', 'TNZL', NULL, NULL, '每日一次', 70.0),
('ORD018', '超声波理疗', '治疗', 'CSBLL', NULL, NULL, '隔日一次', 80.0),
('ORD019', '中药熏蒸', '治疗', 'ZYXZ', NULL, NULL, '每日一次', 90.0),
('ORD020', '高压氧治疗', '治疗', 'GYYZL', NULL, NULL, '每日一次', 150.0);


-- 检验类
INSERT INTO doctororder_dict (ordid, ordername, ordertype, inputstr, drugSpecifications, takeMedicine, Frequency, price) VALUES
('ORD021', '血常规', '检验', 'XCG', NULL, NULL, NULL, 40.0),
('ORD022', '肝功能检查', '检验', 'GGNJC', NULL, NULL, NULL, 55.0),
('ORD023', '肾功能检查', '检验', 'SGNJC', NULL, NULL, NULL, 60.0),
('ORD024', '肿瘤标志物', '检验', 'ZMBZW', NULL, NULL, NULL, 200.0),
('ORD025', '甲状腺功能', '检验', 'JZXGN', NULL, NULL, NULL, 75.0);



CREATE TABLE `doctororder` (
  `orderid` int unsigned NOT NULL AUTO_INCREMENT COMMENT 'CHANGE COLUMN `orderid` `orderid` INT UNSIGNED AUTO_INCREMENT;',
  `Rid` varchar(15) NOT NULL COMMENT '挂号编号',
  `ordername` varchar(80) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL COMMENT '门诊医嘱名称',
  `ordertype` varchar(45) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL COMMENT '门诊医嘱类型',
  `prescriptiontime` datetime DEFAULT NULL COMMENT '医嘱开立时间',
  `orderprice` float DEFAULT NULL COMMENT '单价',
  `ispaid` tinyint DEFAULT NULL COMMENT '是否付费',
  `drugSpecifications` varchar(45) DEFAULT NULL COMMENT '药品规格',
  `orderallprice` float DEFAULT NULL COMMENT '总价',
  `takeMedicine` varchar(45) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL COMMENT '给药途径',
  `Frequency` varchar(45) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL COMMENT '频次',
  `TotalOrder` int DEFAULT NULL COMMENT '总量',
  `patiid` varchar(45) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL COMMENT '患者就诊卡号',
  PRIMARY KEY (`orderid`),
  UNIQUE KEY `orderid_UNIQUE` (`orderid`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3 COMMENT='门诊医嘱表'
;

CREATE TABLE tb_user (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    user_name VARCHAR(255) NOT NULL,
    password VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL
);


### 新建立数据
INSERT INTO tb_user (user_name, password, email) 
VALUES 
    ('admin', '123456', 'zhang.wei@example.com'),
    ('李娜', '123456', 'li.na@example.com'),
    ('王磊', 'Qwerty!2024', 'wang.lei@example.org'),
    ('刘涛', 'Welcome#45', 'liu.tao@company.com'),
    ('陈晨', 'Mys3cureP@ss', 'chen.chen123@gmail.com')
;

INSERT INTO `pati_info_basic` (`Pid`, `Pname`, `Pgender`, `Pidcard`, `Pbirthday`, `Ptel`) VALUES
('P10001', '张三', '男', '110101199001012345', '1990-01-01', '13800001111'),
('P10002', '李四', '女', '110101198805058888', '1988-05-05', '13900002222'),
('P10003', '王五', '男', '110101197703039999', '1977-03-03', '13700003333'),
('P10004', '赵六', '女', '110101200012123456', '2000-12-12', '13600004444'),
('P10005', '孙七', '男', '110101199511112222', '1995-11-11', '13500005555')
;

delete from hospitaldepartment;

-- 内科类科室
INSERT INTO hospitaldepartment (officeid, officename, officetype) VALUES
('0101', '普通内科', '内科'),
('0102', '心血管内科', '内科'),
('0103', '呼吸内科', '内科'),
('0104', '消化内科', '内科'),
('0105', '神经内科', '内科'),
('0106', '内分泌科', '内科'),
('0107', '肾内科', '内科'),
('0108', '血液内科', '内科'),
('0109', '感染科', '内科'),
('0110', '风湿免疫科', '内科');

-- 外科类科室
INSERT INTO hospitaldepartment (officeid, officename, officetype) VALUES
('0201', '普通外科', '外科'),
('0202', '骨科', '外科'),
('0203', '神经外科', '外科'),
('0204', '心胸外科', '外科'),
('0205', '泌尿外科', '外科'),
('0206', '肝胆外科', '外科'),
('0207', '烧伤整形科', '外科'),
('0208', '血管外科', '外科'),
('0209', '乳腺外科', '外科'),
('0210', '肛肠外科', '外科');

insert into hospitaldepartment(officeid,officename,officetype)values('01','内科','内科'),('02','外科','外科')
;

=======
>>>>>>> b65eb77edd33d23bbfd25f1254602c5f3e470f98
