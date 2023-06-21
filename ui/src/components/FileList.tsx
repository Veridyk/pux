import AddCircleIcon from '@mui/icons-material/AddCircle';
import EditIcon from '@mui/icons-material/Edit';
import InsertDriveFileIcon from '@mui/icons-material/InsertDriveFile';
import RemoveCircleIcon from '@mui/icons-material/RemoveCircle';
import { Box, List, ListItem, Typography } from '@mui/material';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import { FileInDirectory } from '../models/FileInDirectory';
import { FileState } from '../models/FileState';

interface FileListData {
  title: string;
  message: string;
  fileList: FileInDirectory[];
}

const FileList = (props: FileListData) => {
  const { title, message, fileList } = props;

  const getIcon = (state: FileState) => {
    switch (state) {
      case FileState.Actual: {
        return <InsertDriveFileIcon />;
      }
      case FileState.Added: {
        return <AddCircleIcon />;
      }
      case FileState.Modified: {
        return <EditIcon />;
      }
      case FileState.Deleted: {
        return <RemoveCircleIcon />;
      }
      default: {
        return <InsertDriveFileIcon />;
      }
    }
  };

  if (!fileList.length) {
    return (
      <Box>
        <Typography sx={{ mb: 2, mt: 4 }} variant="h6">
          {title}
        </Typography>
        <Typography>{message}</Typography>
      </Box>
    );
  }

  return (
    <Box>
      <Typography sx={{ mb: 2, mt: 4 }} variant="h6">
        {title}
      </Typography>
      <List dense={false}>
        {fileList.map((file, index) => {
          return (
            <ListItem key={index}>
              <ListItemIcon>{getIcon(file.state)}</ListItemIcon>
              <ListItemText
                primary={
                  file.state === FileState.Modified
                    ? `${file.filename} (Version ${file.version})`
                    : file.filename
                }
                secondary={file.path}
              />
            </ListItem>
          );
        })}
      </List>
    </Box>
  );
};

export default FileList;
