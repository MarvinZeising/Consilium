<template>
  <v-card v-if="shift" min-width="350px" max-width="500px">
    <v-toolbar dark flat>
      <v-toolbar-title>{{ shift.getTimespan(userModule.getUser) }}</v-toolbar-title>
      <v-spacer></v-spacer>
      <UpdateShiftDialog :shift="getShift" />
      <v-btn icon :title="$t('core.close')" @click="emitClosed">
        <v-icon>close</v-icon>
      </v-btn>
    </v-toolbar>

    <v-card-text style="min-width:350px;" v-t="'shift.status.draftDescription'" />
    <v-card-actions>
      <v-spacer />
      <HandlePlanShiftDialog :shift="shift" />
    </v-card-actions>
  </v-card>
</template>

<style lang="scss" scoped>
.v-toolbar {
  background: repeating-linear-gradient(
    -45deg,
    #222,
    #222 10px,
    var(--v-accent-darken1) 10px,
    var(--v-accent-darken1) 20px
  );
  text-shadow: 2px 2px #222;
}
</style>

<script lang="ts">
import { Vue, Component, Prop, Emit } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import UserModule from '../../store/users'
import UpdateShiftDialog from './UpdateShiftDialog.vue'
import HandlePlanShiftDialog from './HandlePlanShiftDialog.vue'
import { Shift } from '../../models'

@Component({
  components: {
    UpdateShiftDialog,
    HandlePlanShiftDialog,
  },
})
export default class CreateTeamDialog extends Vue {
  private userModule = getModule(UserModule, this.$store)

  @Prop(Shift)
  private readonly shift?: Shift

  @Emit('closed')
  private emitClosed() {}
}
</script>
