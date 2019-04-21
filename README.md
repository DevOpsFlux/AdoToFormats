# AdoToFormats Excute Program
MS DB Procedure Call Data XML/JSON 포맷 파일 생성

## 1. 프로젝트 정보 및 버젼

### *[ AdoToFormats Solution ]	
### *[ AdoToFormats.csproj ]	

| 프로젝트 | 설명 | .NET버젼 | AdoToFormats버젼 |
| -------- | -------- | -------- | -------- |
| AdoToFormats | DB Proc -> XML/JSON 	| .NET 3.5	| AdoToFormats 1.0.0.0 |

## 2. Config 정보
AdoToFormats.config
DB Connection 및 Log Path 설정

## 3. Library 정보
* Newtonsoft.Json.dll 3.5.0.0	

## 4. 프로젝트 설정 및 사용 메뉴얼
* /Doc/AdoToFormats_메뉴얼.pptx

## 5. 프로그램 환경 설정
- AdoToFormats.exe
 > C:\Windows
- AdoToFormats.config 
 > Environment.SystemDirectory
 > C:\Windows\System32
 > C:\Windows\SysWOW64

## 6. 프로그램 실행 정보
> CMD > AdoToFormats /H

| 필수/옵션 | 인수 | 내용 |
| -------- | -------- | -------- |
| 필수 | /F [FORMAT]   | XML, JSON  |
| 필수 | /C [CONNETTION DB]   | 연결 DB -> Config [ DevDB / RealDB]  |
| 필수 | /S [SQLCOMMAND]   | SQL Query 문자열
| 필수 | /O [OUTPUT FILEPATH]   | 파일 저장 경로
| 옵션 | /I [IDENTED OPTION]   | XML, JSON 들여쓰기 Y/N [Default:N]
| 옵션 | /D [CDATA OPTION]   | XML CDATA Y/M(Filed PreFix:CDATA_)/N [Default:N]  |
| 옵션 | /T [COMMAND TIMEOUT]   | SQL TimeOut 설정 [Default:180(초:3분)]  |
| 옵션 | /X [EMPTY]  | 빈 파일생성 Y/N [Default:N (빈 파일생성 안함)]   |
| 옵션 | /H [EXE INFO]  | 실행 옵션 정보  |

## 7. 프로그램 사용 샘플
* /Sample/AdoToFormats_Help.bat
* /Sample/AdoToFormats_JSON.bat
* /Sample/AdoToFormats_XML.bat

* testXML.bat :
```
AdoToFormats /F "XML" /C "TESTDB" /S "Proc_Test7 '1'" /O "D:\Batch\Data\Test\TestXML.xml" /I "Y" /D "M"
AdoToFormats /F "XML" /C "TESTDB" /S "Proc_Test7 '2'" /O "D:\Batch\Data\Test\TestXML2.xml" /I "Y" /D "Y"
AdoToFormats /F "XML" /C "TESTDB" /S "Proc_Test7 '2'" /O "D:\Batch\Data\Test\TestXML3.xml" /I "Y"
AdoToFormats /F "XML" /C "TESTDB" /S "Proc_Test7 '3'" /O "D:\Batch\Data\Test\TestXML4.xml" /I "Y" /D "Y" /X "Y"
```
* testJSON.bat :
```
AdoToFormats /F "JSON" /C "TESTDB" /S "Proc_Test7 '1'" /O "D:\Batch\Data\Test\TestJSON.json" /I "Y"
AdoToFormats /F "JSON" /C "TESTDB" /S "Proc_Test7 '2'" /O "D:\Batch\Data\Test\TestJSON2.json" /I "N"
AdoToFormats /F "JSON" /C "TESTDB" /S "Proc_Test7 '3'" /O "D:\Batch\Data\Test\TestJSON3.json" /I "N" /X "Y“
```
