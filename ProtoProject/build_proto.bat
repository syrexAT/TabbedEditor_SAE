echo Building intermediate
tools\protoc --proto_path=proto proto\worldData.proto --descriptor_set_out=intermediate\worldData.proto.pb
echo Building CSharp
tools\ProtoGen.exe -o:WorldDataDLL\WorldDataDLL\worldData.proto.cs -i:intermediate\worldData.proto.pb
echo Building pyhton
tools\protoc -I=proto --python_out=pythonProto proto/worldData.proto
echo Building Done

cd WorldDataDLL
devenv /build Release WorldDataDLL.sln
cd ..

Copy WorldDataDLL\WorldDataDLL\bin\Release\WorldDataDLL.dll dlls
Copy WorldDataDLL\WorldDataDLL\bin\Release\protobuf-net.dll dlls

cd Precompile
precompile.exe -o:..\dlls\ProtoSerialzer.dll -t:ProtoSerializer ..\dlls\WorldDataDLL.dll
cd ..
