import { Box, Button, Grid, TextField, Typography } from '@mui/material';
import { useRef, useState } from 'react';
import './App.css';
import FileList from './components/FileList';
import { FileInDirectoryDto } from './dto/FileInDirectoryDto';
import { FileInDirectory } from './models/FileInDirectory';

const LIST_URL = 'https://localhost:7273/list?path=';
const DIFF_URL = 'https://localhost:7273/difference?path=';

function App() {
  const [fileList, setFileList] = useState<FileInDirectory[]>([]);
  const [changedList, setChangedList] = useState<FileInDirectory[]>([]);
  const [error, setError] = useState<string>('');
  const pathInputRef = useRef<HTMLInputElement>();

  const getFileList = async (): Promise<void> => {
    const directory = pathInputRef.current?.value;

    if (!directory) {
      setError('Fill the directory please');
      return;
    }

    const response = await fetch(LIST_URL + directory);
    const files = (await response.json()) as FileInDirectoryDto[];

    if (response.status === 200) {
      setFileList(files);
      setError('');
    } else {
      setError('Invalid directory');
    }
  };

  const getDirectoryChanges = async (): Promise<void> => {
    const directory = pathInputRef.current?.value;

    if (!directory) {
      setError('Fill the directory please');
      return;
    }

    const response = await fetch(DIFF_URL + directory);
    const files = (await response.json()) as FileInDirectoryDto[];

    if (response.status === 200) {
      setChangedList(files);
      setError('');
    } else {
      setError('Invalid directory');
    }
  };
  return (
    <Box>
      <Box sx={{ display: 'flex' }}>
        <TextField
          inputRef={pathInputRef}
          size={'small'}
          label="Directory"
          variant="outlined"
          required
        />
        <Button
          sx={{ marginLeft: '10px' }}
          variant="contained"
          onClick={getFileList}
        >
          Load content
        </Button>
        <Button
          sx={{ marginLeft: '10px' }}
          variant="contained"
          onClick={getDirectoryChanges}
        >
          View changes
        </Button>
      </Box>
      <Box sx={{ display: 'flex', my: 2 }}>
        <Typography sx={{ color: 'red' }}>{error}</Typography>
      </Box>
      <Grid container spacing={2}>
        <Grid item xs={6}>
          <FileList
            title="Files in directory"
            fileList={fileList}
            message="No files in directory."
          />
        </Grid>
        <Grid item xs={6}>
          <FileList
            title="Changes in Directory"
            fileList={changedList}
            message="No changes in directory."
          />
        </Grid>
      </Grid>
    </Box>
  );
}

export default App;
