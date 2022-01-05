# MUI Submodule to Top-Level Imports

C# script that replaces all submodule imports with top-level imports.

## Example

From:

```
import Box from '@mui/material/Box';
import Divider from '@mui/material/Divider';
import Typography from '@mui/material/Typography';
```

To:

```
import {
  Box,
  Divider,
  Typography
} from '@mui/material';
```
