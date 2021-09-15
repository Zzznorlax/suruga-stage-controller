# US2440 Controller interface

## 使用介面
![](https://i.imgur.com/zM0ODCW.png)

## 系統連線
輸入AMSNetID並按下Connect鍵將PC連線到US2440 controller
右方訊息欄位會顯示是否完成連線的動作
```csharp=
SetAddress(textBoxAMSNetID.Text);
alignSystem.SetAddress(netAddr);
```

## 開啟伺服馬達
按下Servo鍵開啟伺服馬達
右方訊息欄位會顯示是否完成開啟伺服馬達的動作
```csharp=
private void CheckBoxServo_CheckedChanged(object sender, EventArgs e)
{
    if (CheckBoxServo.Checked)
    {

        for (int i = 0; i < axisNumber; i++)
        {
            axis[i].TurnOnServo();
        }
        CheckBoxServo.BackColor = Color.LightGreen;

        Print("Servo turned on");

    } else
    {
        for (int i = 0; i < axisNumber; i++)
        {
            axis[i].TurnOffServo();
        }
        CheckBoxServo.BackColor = Color.LightCoral;

        Print("Servo turned off");

    }
}
```

## 相對距離移動
輸入Speed調整移動速度
輸入Step length調整單次移動距離
使用"+"和"-"控制移動
```csharp=
private void buttonAxis1Plus_Click(object sender, EventArgs e)
{
    if (TestConnection())
    {
        setAxis1Speed();
        axis[0].MoveRelative(Double.Parse(textBoxAxis1StepLength.Text));
    }
}

private void buttonAxis1Minus_Click(object sender, EventArgs e)
{
    if (TestConnection())
    {
        setAxis1Speed();
        axis[0].MoveRelative(-Double.Parse(textBoxAxis1StepLength.Text));
    }
}
```

## 移動到指定位置
輸入Speed調整移動速度
在Target position內輸入要到達的位置，再按下Go鍵開始移動
```csharp=
private void buttonAxis1Go_Click(object sender, EventArgs e)
{
    setAxis1Speed();
    axis[0].MoveAbsolute(Double.Parse(textBoxAxis1TargetPosition.Text));
}
```

## 回到原點
按下ORG鍵可將滑台移動回原點
```csharp=
private void buttonAxis1Org_Click(object sender, EventArgs e)
{
    if (TestConnection())
    {
        axis[0].ReturnOrigin();
    }
}
```

## 停止移動
按下Stop鍵立即停止滑台移動
```csharp=
private void buttonAxis1Stop_Click(object sender, EventArgs e)
{
    axis[0].Stop();
}
```

## 位置即時追蹤
滑台的位置會在連線成功後開始追蹤並且即時顯示於Current position欄位中
追蹤滑台的線程和其他指令的進行互相獨立
```csharp=
private void backgroundWorkerAlignmentStatus_DoWork(object sender, DoWorkEventArgs e)
{
    while (true)
    {
        labelAlignmentStatus.Invoke((MethodInvoker)delegate
        {
            labelAlignmentStatus.Text = alignment.GetStatus().ToString();
        });

        labelPower.Invoke((MethodInvoker)delegate
        {
            labelPower.Text = alignment.GetPower(flatParam.analogCh).ToString();
        });

        labelVoltage.Invoke((MethodInvoker)delegate
        {
            labelVoltage.Text = alignment.GetVoltage(flatParam.analogCh).ToString();
        });

        System.Threading.Thread.Sleep(500);
    }
}
```


## 耦光
### 參數調整
提供使用者調整9種耦光系統可使用的參數
```csharp=
private void setFlatAlignmentParam()
{
    flatParam.mainStageNumberX = (uint) numericUpDownX.Value;
    flatParam.mainStageNumberY = (uint) numericUpDownY.Value;
    flatParam.subStageNumberXY = 0;
    flatParam.subAngleX = 0.0;
    flatParam.subAngleY = 0.0;
    flatParam.pmCh = (uint) numericUpDownPMCH.Value;
    flatParam.analogCh = (uint) numericUpDownAnalogCH.Value;
    flatParam.pmAutoRangeUpOn = false;
    flatParam.pmInitRangeSettingOn = false;
    flatParam.pmInitRange = -20;
    flatParam.fieldSearchThreshold = (double) numericUpDownFieldSearch.Value;
    flatParam.peakSearchThreshold = (double) numericUpDownPeakSearch.Value;
    flatParam.searchRangeX = (double) numericUpDownSearchRangeX.Value;
    flatParam.searchRangeY = (double) numericUpDownSearchRangeY.Value;
    flatParam.fieldSearchPitchX = (double) numericUpDownPitchX.Value;
    flatParam.fieldSearchPitchY = (double) numericUpDownPitchY.Value;
    flatParam.fieldSearchFirstPitchX = 0;
    flatParam.fieldSearchSpeedX = (double) numericUpDownFieldSearchSpeedX.Value;
    flatParam.fieldSearchSpeedY = (double) numericUpDownFieldSearchSpeedY.Value;
    flatParam.peakSearchSpeedX = (double) numericUpDownPeakSearchSpeedX.Value;
    flatParam.peakSearchSpeedY = (double) numericUpDownPeakSearchSpeedY.Value;
    flatParam.smoothingRangeX = 50;
    flatParam.smoothingRangeY = 50;
    flatParam.centroidThresholdX = 0;
    flatParam.centroidThresholdY = 0;
    flatParam.convergentRangeX = 0.5;
    flatParam.convergentRangeY = 0.5;
    flatParam.comparisonCount = 2;
    flatParam.maxRepeatCount = 10;
}

```

### 開始耦光
調整後按下Start鍵可開始進行耦光
```csharp=
private void buttonStartAlignment_Click(object sender, EventArgs e)
{
    setFlatAlignmentParam();
    alignment.SetFlat(flatParam);

    Print(String.Format("Start aligning on stage {0} and {1}", flatParam.mainStageNumberX.ToString(), flatParam.mainStageNumberY.ToString()));
    alignment.StartFlat();
}
```

### 停止耦光
按下Stop鍵可停止耦光
```csharp=
private void buttonAligmnetStop_Click(object sender, EventArgs e)
{
    alignment.Stop();
}

```

### 耦光狀態顯示
Measurments中會顯示測量到的Voltage和Power
Status會顯示耦光系統目前運行的狀態
```csharp=
private void backgroundWorkerAlignmentStatus_DoWork(object sender, DoWorkEventArgs e)
{
    while (true)
    {
        labelAlignmentStatus.Invoke((MethodInvoker)delegate
        {
            labelAlignmentStatus.Text = alignment.GetStatus().ToString();
        });

        labelPower.Invoke((MethodInvoker)delegate
        {
            labelPower.Text = alignment.GetPower(flatParam.analogCh).ToString();
        });

        labelVoltage.Invoke((MethodInvoker)delegate
        {
            labelVoltage.Text = alignment.GetVoltage(flatParam.analogCh).ToString();
        });

        System.Threading.Thread.Sleep(500);
    }
}

```
