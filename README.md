# BatchRename
- Họ và tên: Nguyễn Huy Tùng
- MSSV: 19120422
- Chú ý khi compile và chạy chương trình:
	+ Cần tạo folder "Preset" trong folder thực thi chương trình trước.
- Các chức năng đã thực hiện:
	+ Dynamically load all renaming rules from external DLL files
	+ Select all files you want to rename
	+ Create a set of rules for renaming the files. 
    		1. Each rule can be added from a menu 
    		2. Each rule's parameters can be edited
	+ Apply the set of rules in numerical order to each file, make them have a new name
	+ Save this set of rules into presets for quickly loading later if you need to reuse
- Các chức năng chưa thực hiện:
	+ Select folders to rename
- Các Renaming rules đã thực hiện:
	1. Change the extension to another extension (no conversion, force renaming extension)
	2. Add counter to the end of the file
		+ Could specify the start value, steps, number of digits 
		(Could have padding like 01, 02, 03...10....99)
	3. Remove all space from the beginning and the ending of the filename
	4. Replace certain characters into one character like replacing "-" ad "_" into space " "
	5. Adding a prefix to all the files
	6. Adding a suffix to all the files
	7. Convert all characters to lowercase, remove all spaces
	8. Convert filename to PascalCase
- Các Improvements đã thực hiện:
	+ Drag & Drop a file to add to the list
	+ Handling duplication
	+ Let the user see the preview of the result
- Bonus: sử dụng mẫu Factory, prototype
- Expected grade: 10
- Video demo: https://youtu.be/nz_OufqVeVU
