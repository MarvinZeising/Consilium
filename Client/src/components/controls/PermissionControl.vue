<template>
  <div>
    <p v-t="translationPath + 'Description'" />
    <v-select
      v-model="model.value"
      :items="permissionValues"
      item-value="value"
      :label="$tc(translationPath, 2)"
      filled
      required
    >
      <template v-slot:selection="{ item }">
        <span>{{ $t('core.' + item.value) }}</span>
      </template>
      <template v-slot:item="{ item }">
        <span>{{ $t('core.' + item.value) }}</span>
      </template>
    </v-select>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'

@Component
export default class PermissionControl extends Vue {

  @Prop(Object)
  private readonly model: { value: string } = { value: 'none' }

  @Prop(String)
  private readonly translationPath: string = ''

  private permissionValues: any[] = [ 'none', 'read', 'write' ].map((value) => {
    return { value }
  })

}
</script>
