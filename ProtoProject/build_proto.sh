files=''
for file in `ls ../core/pb2/*.proto | xargs -n1 basename`
do
	files="$files"-i:intermediate/"$file".pb" "
	protoc --proto_path=../core/pb2 --proto_path=/opt/local/include ../core/pb2/"$file" --descriptor_set_out=intermediate/"$file".pb
	mono tools/ProtoGen.exe -o:ProtocDLL/ProtocDLL/"$file".cs -i:intermediate/"$file".pb
done
for file in `ls ../was/pb2/*.proto | xargs -n1 basename`
do

	files="$files"-i:intermediate/"$file".pb" "
	protoc --proto_path=../core/pb2 --proto_path=../was/pb2 --proto_path=/opt/local/include ../was/pb2/"$file" --descriptor_set_out=intermediate/"$file".pb
	mono tools/ProtoGen.exe -o:ProtocDLL/ProtocDLL/"$file".cs -i:intermediate/"$file".pb
done

mdtool build -c:Release ProtocDLL/ProtocDLL/ProtocDLL.sln
cp -f ProtocDLL/ProtocDLL/bin/Release/ProtocDLL.dll dlls/
cp -f ProtocDLL/ProtocDLL/bin/Release/protobuf-net.dll dlls/
mono Precompile/precompile.exe -o:dlls/ProtoSerialzer.dll -t:ProtoSerializer dlls/ProtocDLL.dll
cp -f dlls/*.dll ../was/sfe/Assets/Plugins/

