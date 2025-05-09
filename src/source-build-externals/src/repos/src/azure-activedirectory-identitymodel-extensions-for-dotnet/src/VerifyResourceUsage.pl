######################################################################
# VerifyResourceUsage.pl
######################################################################
# Run like this:
#    perl VerifyResourceUsage.pl
# To discover 
#    1) what LogMessages.cs strings are unused
#    2) whether any of the string IDs and string value prefixes don't match
#    3) whether any LogMessages.cs files contain duplicate IDs
#    4) whether any commented out IDs have been accidentally reused

use strict;
use strict 'subs';
use strict 'vars';
use strict 'refs';
use Cwd;
use Data::Dumper;
use File::Find;
use File::Basename;

my $PERF_DEBUG = (1==0); # print debug info to screen
my $ERRORS_ONLY_MODE = (1==0);  # only print serious errors

# prefix strings
my $ERROR = "VerifyResourceUsage.pl: error";
my $WARN = "VerifyResourceUsage.pl: warning";
my $EXITCODE = 0;

my $CROP_FILE_PATH_AT = "src";

######################################################################
sub PrintError($)
{
    my $s = shift;
    $EXITCODE = -1;
    if ($ERRORS_ONLY_MODE)
    {
        print "$ERROR $s";
    }
    else
    {
        print "$s";
    }
}

sub PrintWarn($)
{
    my $s = shift;
    if ($ERRORS_ONLY_MODE)
    {
        print "$WARN $s";
    }
    else
    {
        print "$s";
    }
}

sub Print($)
{
    my $s = shift;
    if ($ERRORS_ONLY_MODE)
    {
        # do nothing, this is just informational
    }
    else
    {
        print "$s";
    }
}

sub PrintPerf($)
{
    my $s = shift;
    if ($PERF_DEBUG)
    {
        print "$s";
    }
}

######################################################################

# Each format item takes the following form.
#
# {index[,alignment][:formatString]}
#
# The matching braces ("{" and "}") are required. Because opening and closing braces are interpreted as 
# starting and ending a format item, you must specify two opening braces ("{{") in the fixed text to 
# display one opening brace ("{"), and specify two closing braces ("}}") in the fixed text to display 
# one closing brace ("}").

my $FORMAT_STR_ARG = qr/
                        (?<!{)         # not preceded by {
                        \{             # {
                        (\d+)          # arg# SAVED IN $1
                        (?:,-?\d+)?    # optional alignment (signed integer)
                        (?::[^}]*)?    # optional formatString
                        }              # }
                        /ox;

sub CountArgs($$)
{
    my $id = shift;
    my $formatStr = shift;
    my %argNums;
    if ($formatStr =~ /$FORMAT_STR_ARG/)
    {
        while ($formatStr =~ /$FORMAT_STR_ARG/g)
        {
            my $argNum = $1;
            $argNums{$argNum} = 1;
        }

        my @argNums = sort keys %argNums;
        my $numArgs = scalar(@argNums);
        if ($argNums[-1] != ($numArgs-1))
        {
            PrintError("\n");
            PrintError("$id doesn't use arguments properly: $formatStr\n");
        }

        return $numArgs;
    }
    else
    {
        return 0;
    }
}

sub VerifyNumBraces($$)
{
    my $id = shift;
    my $formatString = shift;
    my @braces;

    foreach my $char (split //, $formatString) {
        if($char eq "{") {
            push @braces, $char
        } elsif ($char eq "}") {
            if (@braces == 0) {
                PrintError("The following format string has mismatching braces.\n");
                PrintError("$id =$formatString\n\n");
                return;
            } else {
                pop @braces
            }
        }
    }

    if (@braces != 0) {
          PrintError("The following format string has mismatching braces.\n");
          PrintError("$id = $formatString\n\n");
    }
}

######################################################################
# Main 

# stores the file names for each directory
my @filenames;

# stores the log message ids for each directory
my %dirhash;

# stores all directories that contain a LogMessages.cs file
my @directories;

# stores all used log message ids across every directory
my %allids; 

# stores commented out log message ids
my %commentedids;

# get all the directories that we need to check LogMessages.cs for
my $dir = getcwd;

sub FindDirs {
    my $file = $_;
    return unless -f $file;
    my $fullpath = $File::Find::name;
    if ($file =~ /LogMessages.cs$/) {
        my $directory = dirname($fullpath);    
        push @directories, $directory;
    }
}

find(\&FindDirs, $dir);

# first we need to go through each directory and find all log message ids
foreach my $directory (@directories) 
{
    my $resources_txt = $directory . "/LogMessages.cs";

    open(RES, "< $resources_txt") or die "can't open $resources_txt: $!";

    while (<RES>)
    {
        next if /^;/;    # lines starting with ';' are comments
        next if /^#/;    # lines starting with '#' are comments
        next if not /(\/\/)?\s*(internal|public) const string (\w*?)\s*=(.*)/;
        my $comment = $1; # whether or not this log message has been commented out   
        my $scope = $2; # whether the identifier is public or internal
        my $id = $3;     # identifier is everything up to '='
        my $value = $4;  # rest is actual resource string value

        if ($comment eq "") # the id is not commented out
        {
            $dirhash{$directory}{$id} = [0, $value];
        } 
        else {
            $commentedids{$id} = 1;
        }
        
    }
    
    close(RES) or die "can't close $resources_txt: $!";

    PrintPerf("\nPERF: Open Files\n");

    @filenames = ();

    sub FindFilenames {
        my $file = $_;
        return unless -f $file;
        my $fullpath = $File::Find::name;
        if ($file =~ /(.)+.cs$/) {
            push @filenames, $fullpath;
        }
    }

    # find all the files in the current directory
    find(\&FindFilenames, $directory);

    foreach my $file (@filenames)
    {
        $file =~ /$CROP_FILE_PATH_AT(.*)/;
        my $shortFileName = $1;

        open(FIL, "< $file") or die "can't open $file: $!";
    
        my $filestring;
        {
            local $/; # slurp
            $filestring = <FIL>; #slurp!
        }
        close(FIL) or die "can't close $file: $!";

        # find all call sites
        my $rest = $filestring;

        while ($rest =~ /LogMessages\.(\w+)/g)
        {   
            my $id = $1;

            if (defined $allids{$id})
            {
                $allids{$id}++;   # increment the number of references to this id
            }
            else
            {
                $allids{$id} = 1;
            }
        }
    }
    PrintPerf("\nPERF: done reading and processing source files\n");
}

# go through all the directories again to discover unused, mismatching, and/or duplicate log message ids
foreach my $directory (keys(%dirhash)) 
{
    my $resources_txt = $directory . "/LogMessages.cs";
    PrintPerf("\nPERF: starting script\n");
    PrintPerf("\nPERF: Analyzing file: $resources_txt\n");

    Print("$resources_txt\n\n");
    PrintPerf("\nPERF: reading resources\n");
    
    PrintPerf("\nPERF: checking string id values\n");

    # go through all log ids in this directory
    foreach my $id(keys %{ $dirhash{$directory} }) 
    {
        # if we found a commented out log message with the same id
        if (defined($commentedids{$id}))
        {
            PrintError("\nSame id ($id) is reused. \n\n");  
        }
        
        # go through each directory that we've processed so far and check for duplicates and reused log messages
        foreach my $directory2 (keys(%dirhash))
        {
            if (grep {$_ eq $id} keys %{ $dirhash{$directory2}} ) 
            {
                if (not $directory eq $directory2) 
                {
                    PrintError("\nSame id ($id) is reused in the LogMessages.cs file of both: \n $directory \n and \n $directory2 \n \n");  
                }
            } 
        } 

        CountArgs($id, $dirhash{$directory}{$id}[1]);
        VerifyNumBraces($id, $dirhash{$directory}{$id}[1]);
        my $length = length($id); # get length of the log message id
        my $stringvalue = substr ($dirhash{$directory}{$id}[1], 2); # remove quote char at start of string
        my $stringid = substr($stringvalue, 0, $length); # get prefix of string value
        if (not $id eq $stringid) 
        {
            PrintError("string name ($id) is not the same as it's value: $stringid \n \n");
        } 

    }

    PrintPerf("\nPERF: done reading resources\n");

    PrintPerf("\nDone processing files\n");

    my $numUnreferencedIds = 0;
    foreach my $id(sort keys %{ $dirhash{$directory} }) 
    { 
        # if the log message id has not been referenced anywhere
        if (not defined($allids{$id}))
        {
            PrintError("$id was referenced in 0 files\n");
            $numUnreferencedIds++;
        }
    }
        
    Print("\n$numUnreferencedIds identifiers were unreferenced\n\n");

    Print("--------------\n\n");
}

exit $EXITCODE;
