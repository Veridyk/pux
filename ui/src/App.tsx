import { Box, Button, Grid, TextField, Typography } from '@mui/material';
import { useRef, useState } from 'react';
import './App.css';
import FileList from './components/FileList';
import {ErrorResponse} from "./dto/ErrorResponse";
import { FileInDirectoryDto } from './dto/FileInDirectoryDto';
import { FileInDirectory } from './models/FileInDirectory';

const LIST_URL = import.meta.env.VITE_LIST_URL;
const DIFF_URL = import.meta.env.VITE_DIFF_URL;

type setCallbackType = (data: FileInDirectory[]) => void;

function App() {
  const [directoryContent, setDirectoryContent] = useState<FileInDirectory[]>([]);
  const [directoryChangedContent, setDirectoryChangedContent] = useState<FileInDirectory[]>([]);
  const [errorMessage, setErrorMessage] = useState<string>('');
  const directoryInputRef = useRef<HTMLInputElement>();

  const getFileListResponse = async (url: string, setDataCallback : setCallbackType) : Promise<void> => {
    const directory = directoryInputRef.current?.value;

    if (!directory) {
      setErrorMessage('Fill the directory please');
      return;
    }

    const response = await fetch(url + directory);
    const body = await response.json();

    if (response.status === 200) {
      const files = body as FileInDirectoryDto[];
      setDataCallback(files);
      setErrorMessage('');
    } else {
      const error = body as ErrorResponse;
      setErrorMessage(error.message);
    }
  }

  return (
    <Box>
      <Box sx={{ display: 'flex' }}>
        <TextField
          inputRef={directoryInputRef}
          size={'small'}
          label="Directory"
          variant="outlined"
          required
        />
        <Button
          sx={{ marginLeft: '10px' }}
          variant="contained"
          onClick={() => getFileListResponse(LIST_URL, setDirectoryContent)}
        >
          Load content
        </Button>
        <Button
          sx={{ marginLeft: '10px' }}
          variant="contained"
          onClick={() => getFileListResponse(DIFF_URL, setDirectoryChangedContent)}
        >
          View changes
        </Button>
      </Box>
      <Box sx={{ display: 'flex', my: 2 }}>
        <Typography sx={{ color: 'red' }}>{errorMessage}</Typography>
      </Box>
      <Grid container spacing={2}>
        <Grid item xs={6}>
          <FileList
            title="Files in directory"
            directoryContent={directoryContent}
            message="No files in directory."
          />
        </Grid>
        <Grid item xs={6}>
          <FileList
            title="Changes in Directory"
            directoryContent={directoryChangedContent}
            message="No changes in directory."
          />
        </Grid>
      </Grid>
    </Box>
  );
}

export default App;
