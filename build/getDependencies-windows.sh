#!/bin/bash
# server=build.palaso.org
# project=HearThis
# build=HearThis-Win-master-Continuous
# root_dir=..
# Auto-generated by https://github.com/chrisvire/BuildUpdate.
# Do not edit this file by hand!

cd "$(dirname "$0")"

# *** Functions ***
force=0
clean=0

while getopts fc opt; do
case $opt in
f) force=1 ;;
c) clean=1 ;;
esac
done

shift $((OPTIND - 1))

copy_auto() {
if [ "$clean" == "1" ]
then
echo cleaning $2
rm -f ""$2""
else
where_curl=$(type -P curl)
where_wget=$(type -P wget)
if [ "$where_curl" != "" ]
then
copy_curl "$1" "$2"
elif [ "$where_wget" != "" ]
then
copy_wget "$1" "$2"
else
echo "Missing curl or wget"
exit 1
fi
fi
}

copy_curl() {
echo "curl: $2 <= $1"
if [ -e "$2" ] && [ "$force" != "1" ]
then
curl -# -L -z "$2" -o "$2" "$1"
else
curl -# -L -o "$2" "$1"
fi
}

copy_wget() {
echo "wget: $2 <= $1"
f1=$(basename $1)
f2=$(basename $2)
cd $(dirname $2)
wget -q -L -N "$1"
# wget has no true equivalent of curl's -o option.
# Different versions of wget handle (or not) % escaping differently.
# A URL query is the only reason why $f1 and $f2 should differ.
if [ "$f1" != "$f2" ]; then mv $f2\?* $f2; fi
cd -
}


# *** Results ***
# build: HearThis-Win-master-Continuous (HearThis_HearThisWinMasterContinuous)
# project: HearThis
# URL: http://build.palaso.org/viewType.html?buildTypeId=HearThis_HearThisWinMasterContinuous
# VCS: https://github.com/sillsdev/HearThis.git [master]
# dependencies:
# [0] build: HearThis Static Dependencies (HearThis_HearThisStaticDependencies)
#     project: HearThis
#     URL: http://build.palaso.org/viewType.html?buildTypeId=HearThis_HearThisStaticDependencies
#     clean: false
#     revision: latest.lastSuccessful
#     paths: {"Lame.zip!**"=>"DistFiles/lame"}
# [1] build: NetSparkle Continuous (NetSparkle_NetSparkleContinuous)
#     project: NetSparkle
#     URL: http://build.palaso.org/viewType.html?buildTypeId=NetSparkle_NetSparkleContinuous
#     clean: false
#     revision: latest.lastSuccessful
#     paths: {"*.dll"=>"lib/dotnet"}
#     VCS: https://github.com/sillsdev/NetSparkle [master]
# [2] build: palaso-win32-master-nostrongname Continuous (Libpalaso_PalasoWin32masterNostrongnameContinuous)
#     project: libpalaso
#     URL: http://build.palaso.org/viewType.html?buildTypeId=Libpalaso_PalasoWin32masterNostrongnameContinuous
#     clean: false
#     revision: latest.lastSuccessful
#     paths: {"Ionic.Zip.dll"=>"lib/dotnet", "L10NSharp.dll"=>"lib/dotnet", "L10NSharp.pdb"=>"lib/dotnet", "SIL.Core.dll"=>"lib/dotnet", "SIL.Core.pdb"=>"lib/dotnet", "SIL.Core.Desktop.dll"=>"lib/dotnet", "SIL.Core.Desktop.pdb"=>"lib/dotnet", "SIL.DblBundle.dll"=>"lib/dotnet", "SIL.DblBundle.pdb"=>"lib/dotnet", "SIL.Media.dll"=>"lib/dotnet", "SIL.Media.pdb"=>"lib/dotnet", "SIL.Scripture.dll"=>"lib/dotnet", "SIL.Scripture.pdb"=>"lib/dotnet", "SIL.Windows.Forms.dll"=>"lib/dotnet", "SIL.Windows.Forms.pdb"=>"lib/dotnet", "SIL.Windows.Forms.DblBundle.dll"=>"lib/dotnet", "SIL.Windows.Forms.DblBundle.pdb"=>"lib/dotnet", "SIL.WritingSystems.dll"=>"lib/dotnet", "SIL.WritingSystems.pdb"=>"lib/dotnet", "irrKlang.NET4.dll"=>"lib/dotnet"}
#     VCS: https://github.com/sillsdev/libpalaso.git [master]

# make sure output directories exist
mkdir -p ../DistFiles/lame
mkdir -p ../Downloads
mkdir -p ../lib/dotnet

# download artifact dependencies
copy_auto http://build.palaso.org/guestAuth/repository/download/HearThis_HearThisStaticDependencies/latest.lastSuccessful/Lame.zip ../Downloads/Lame.zip
copy_auto http://build.palaso.org/guestAuth/repository/download/NetSparkle_NetSparkleContinuous/latest.lastSuccessful/NetSparkle.Net40.dll ../lib/dotnet/NetSparkle.Net40.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/Libpalaso_PalasoWin32masterNostrongnameContinuous/latest.lastSuccessful/Ionic.Zip.dll ../lib/dotnet/Ionic.Zip.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/Libpalaso_PalasoWin32masterNostrongnameContinuous/latest.lastSuccessful/L10NSharp.dll ../lib/dotnet/L10NSharp.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/Libpalaso_PalasoWin32masterNostrongnameContinuous/latest.lastSuccessful/L10NSharp.pdb ../lib/dotnet/L10NSharp.pdb
copy_auto http://build.palaso.org/guestAuth/repository/download/Libpalaso_PalasoWin32masterNostrongnameContinuous/latest.lastSuccessful/SIL.Core.dll ../lib/dotnet/SIL.Core.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/Libpalaso_PalasoWin32masterNostrongnameContinuous/latest.lastSuccessful/SIL.Core.pdb ../lib/dotnet/SIL.Core.pdb
copy_auto http://build.palaso.org/guestAuth/repository/download/Libpalaso_PalasoWin32masterNostrongnameContinuous/latest.lastSuccessful/SIL.Core.Desktop.dll ../lib/dotnet/SIL.Core.Desktop.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/Libpalaso_PalasoWin32masterNostrongnameContinuous/latest.lastSuccessful/SIL.Core.Desktop.pdb ../lib/dotnet/SIL.Core.Desktop.pdb
copy_auto http://build.palaso.org/guestAuth/repository/download/Libpalaso_PalasoWin32masterNostrongnameContinuous/latest.lastSuccessful/SIL.DblBundle.dll ../lib/dotnet/SIL.DblBundle.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/Libpalaso_PalasoWin32masterNostrongnameContinuous/latest.lastSuccessful/SIL.DblBundle.pdb ../lib/dotnet/SIL.DblBundle.pdb
copy_auto http://build.palaso.org/guestAuth/repository/download/Libpalaso_PalasoWin32masterNostrongnameContinuous/latest.lastSuccessful/SIL.Media.dll ../lib/dotnet/SIL.Media.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/Libpalaso_PalasoWin32masterNostrongnameContinuous/latest.lastSuccessful/SIL.Media.pdb ../lib/dotnet/SIL.Media.pdb
copy_auto http://build.palaso.org/guestAuth/repository/download/Libpalaso_PalasoWin32masterNostrongnameContinuous/latest.lastSuccessful/SIL.Scripture.dll ../lib/dotnet/SIL.Scripture.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/Libpalaso_PalasoWin32masterNostrongnameContinuous/latest.lastSuccessful/SIL.Scripture.pdb ../lib/dotnet/SIL.Scripture.pdb
copy_auto http://build.palaso.org/guestAuth/repository/download/Libpalaso_PalasoWin32masterNostrongnameContinuous/latest.lastSuccessful/SIL.Windows.Forms.dll ../lib/dotnet/SIL.Windows.Forms.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/Libpalaso_PalasoWin32masterNostrongnameContinuous/latest.lastSuccessful/SIL.Windows.Forms.pdb ../lib/dotnet/SIL.Windows.Forms.pdb
copy_auto http://build.palaso.org/guestAuth/repository/download/Libpalaso_PalasoWin32masterNostrongnameContinuous/latest.lastSuccessful/SIL.Windows.Forms.DblBundle.dll ../lib/dotnet/SIL.Windows.Forms.DblBundle.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/Libpalaso_PalasoWin32masterNostrongnameContinuous/latest.lastSuccessful/SIL.Windows.Forms.DblBundle.pdb ../lib/dotnet/SIL.Windows.Forms.DblBundle.pdb
copy_auto http://build.palaso.org/guestAuth/repository/download/Libpalaso_PalasoWin32masterNostrongnameContinuous/latest.lastSuccessful/SIL.WritingSystems.dll ../lib/dotnet/SIL.WritingSystems.dll
copy_auto http://build.palaso.org/guestAuth/repository/download/Libpalaso_PalasoWin32masterNostrongnameContinuous/latest.lastSuccessful/SIL.WritingSystems.pdb ../lib/dotnet/SIL.WritingSystems.pdb
copy_auto http://build.palaso.org/guestAuth/repository/download/Libpalaso_PalasoWin32masterNostrongnameContinuous/latest.lastSuccessful/irrKlang.NET4.dll ../lib/dotnet/irrKlang.NET4.dll
# extract downloaded zip files
unzip -uqo ../Downloads/Lame.zip -d "../DistFiles/lame"
# End of script
