import { FileState } from '../models/FileState';

export interface FileInDirectoryDto {
  filename: string;
  path: string;
  state: FileState;
  version: number;
}
