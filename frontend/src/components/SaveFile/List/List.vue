<template>
    <div class="row">
        <h1>Save Files</h1>
        <table v-if="loaded" class="table table-striped">
            <thead>
                <th>Path</th>
                <th>Last Modified</th>
            </thead>
            <tbody>
                <tr v-bind:key="item.filename" v-for="item in items">
                    <td><router-link v-bind:to="{name: 'SaveFileRead', params: { filename: item.filename }}">{{ item.filename }}</router-link></td>
                    <td>{{ item.lastModified }}</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script src="@/components/SaveFile/List/List.js"></script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss" src="@/components/SaveFile/List/List.scss"></style>
