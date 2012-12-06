## Video for G1 ##

It's an application for encoding video,and the target is just G1(Dream),which is my mobile phone.

2012.12.06
-fix bug
When set the drive letter as the output directory,the outputText will have two '\'s,which leads to the failure of removing intermediate files.

2012.11.27
-refactor
remove interface.
make it clearly.

2012.07.30
-add some functions
add TopMost CheckBox.
add AfterDone function.

2012.07.29
-code refactoring
create VideoItem.class,VideoService.class and FileService.class.
move the function method to VideoService and FileService.
add file data to VideoItem.
